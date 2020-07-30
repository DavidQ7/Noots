import { Label } from '../models/label';
import { State, Action, StateContext, Selector } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { ApiServiceLabels } from '../api.service';
import { LoadLabels, AddLabel, SetDeleteLabel, UpdateLabel, PositionLabel, DeleteLabel, RestoreLabel } from './labels-actions';
import { tap } from 'rxjs/operators';
import { patch, append, removeItem, insertItem, updateItem } from '@ngxs/store/operators';
import { OrderService } from 'src/app/shared/services/order.service';

interface LabelState {
    labelsAll: Label[];
    labelsDeleted: Label[];
}

@State<LabelState>({
    name: 'Labels',
    defaults: {
        labelsAll: [],
        labelsDeleted: []
    }
})

@Injectable()
export class LabelStore {


    constructor(private api: ApiServiceLabels,
                private orderService: OrderService) {
    }


    @Selector()
    static all(state: LabelState): Label[] {
        return state.labelsAll;
    }

    @Selector()
    static deleted(state: LabelState): Label[] {
        return state.labelsDeleted;
    }

    @Action(LoadLabels)
    loadContent({ setState, getState, patchState }: StateContext<LabelState>) {
        if (getState().labelsAll.length === 0) {
        return this.api.getAll().pipe(tap(content => { patchState({
            labelsAll: content.labelsAll,
            labelsDeleted: content.labelsDeleted
         }); }));
        }
    }

    @Action(AddLabel)
    async newLabel({ setState, getState, patchState }: StateContext<LabelState>, { name, color }: AddLabel) {
        const id = await this.api.new(name, color).toPromise();

        const labels = [...getState().labelsAll];
        const newLabels = labels.map((value) => ({...value, order: value.order + 1 }));

        patchState({
            labelsAll: [{name, color, id, isDeleted: false, order: 1}, ...newLabels]
        });
    }

    @Action(SetDeleteLabel)
    async setDeletedLabel({setState, getState, patchState}: StateContext<LabelState>, { id }: SetDeleteLabel) {
        await this.api.setDeleted(id).toPromise();

        // Todo decrement for all lables

        let labelsAll = getState().labelsAll;
        const label = labelsAll.find(x => x.id === id);
        labelsAll = labelsAll.filter(x => x.id !== id);

        const labelsDeleted = [...getState().labelsDeleted];
        const newLabelsDeleted = labelsDeleted.map((value) => ({...value, order: value.order + 1 }));

        patchState({labelsAll, labelsDeleted: [{...label, order: 1}, ...newLabelsDeleted]});
    }

    @Action(DeleteLabel)
    async DeleteLabel({setState, getState, patchState}: StateContext<LabelState>, { id }: DeleteLabel) {
        await this.api.delete(id).toPromise();
        let labelsDeleted = getState().labelsDeleted;

        const labelOrder = labelsDeleted.find(x => x.id === id).order;

        labelsDeleted = labelsDeleted.filter(x => x.id !== id);

        labelsDeleted = labelsDeleted.map(x => {
            if (x.order > labelOrder) {
                return {...x, order: x.order - 1};
            }
            return {...x};
        });


        patchState({labelsDeleted});
    }

    @Action(UpdateLabel)
    async updateLabels({ setState}: StateContext<LabelState>, { label }: UpdateLabel) {
        await this.api.update(label).toPromise();
        if (label.isDeleted) {
            setState(
                patch({
                    labelsDeleted: updateItem<Label>(label2 => label2.id === label.id , label)
                })
            );
        } else {
            setState(
                patch({
                    labelsAll: updateItem<Label>(label2 => label2.id === label.id , label)
                })
            );
        }
    }

    @Action(PositionLabel)
    async positionLabel({setState, getState, patchState}: StateContext<LabelState>, { deleted, id, order }: PositionLabel) {
        await this.orderService.changeOrder(order).toPromise();
        if (deleted) {
            let labelsDeleted = getState().labelsDeleted;
            const slabel = labelsDeleted.find(x => x.id === id);
            labelsDeleted = labelsDeleted.filter(x => x.id !== id);
            labelsDeleted.splice(order.position - 1, 0 , slabel);
            patchState({labelsDeleted});
        } else {
            let labelsAll = getState().labelsAll;
            const slabel = labelsAll.find(x => x.id === id);
            labelsAll = labelsAll.filter(x => x.id !== id);
            labelsAll.splice(order.position - 1, 0 , slabel);
            patchState({labelsAll});
        }
    }

    @Action(RestoreLabel)
    async restoreLabel({setState, getState, patchState}: StateContext<LabelState>, { id }: RestoreLabel) {

        let deletedLables = getState().labelsDeleted;
        const restoreLabel = deletedLables.find(x => x.id === id);
        deletedLables = deletedLables.filter(x => x.id !== id);

        deletedLables = deletedLables.map(x => {
            if (x.order > restoreLabel.order) {
                return {...x, order: x.order - 1};
            }
            return {...x};
        });
        patchState({labelsAll: [restoreLabel, ...getState().labelsAll], labelsDeleted: deletedLables });
    }
}
