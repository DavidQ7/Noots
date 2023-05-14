import { BaseFile } from './base-file';
import { ContentModelBase } from './content-model-base';

export abstract class BaseCollection<T extends BaseFile> extends ContentModelBase {
  name?: string;

  items: T[];

  isLoading = false;

  get getItems(): T[] {
    return this.items;
  }

  get orderedItems(): T[] {
    return this.items.sort((a, b) => a.uploadAt.getTime() - b.uploadAt.getTime());
  }

  get orderedThreeItems(): T[] {
    let items = this.getItems;
    if (items.length > 3) {
      items = items.slice(0, 3);
    }
    return items.sort((a, b) => a.uploadAt.getTime() - b.uploadAt.getTime());
  }

  isEqual(content: BaseCollection<T>): boolean {
    return this.isEqualCollectionInfo(content) && this.getIsEqualIdsToAddIdsToRemove(content)[0];
  }

  // eslint-disable-next-line class-methods-use-this
  isIdsEquals(ids1: string[], ids2: string[]): boolean {
    const res1 = ids1.filter((name) => !ids2.includes(name));
    const res2 = ids2.filter((name) => !ids1.includes(name));
    return res1.length === 0 && res2.length === 0;
  }

  getIsEqualIdsToAddIdsToRemove(content: BaseCollection<T>): [boolean, T[], T[]] {
    const ids1 = content.items.map((x) => x.fileId);
    const ids2 = this.items.map((x) => x.fileId);

    const idsToRemove = ids1.filter((name) => !ids2.includes(name));
    const idsToAdd = ids2.filter((name) => !ids1.includes(name));

    if (idsToAdd.length !== 0 || idsToRemove.length !== 0) {
      const itemsToAdd = this.items.filter((x) => idsToAdd.some((q) => q === x.fileId));
      const itemsToRemove = content.items.filter((x) => idsToRemove.some((q) => q === x.fileId));
      return [false, itemsToAdd, itemsToRemove];
    }

    for (const item of this.items) {
      const itemSecond = content.items.find((x) => x.fileId === item.fileId);
      if (!item.isEqual(itemSecond)) {
        return [false, null, null];
      }
    }

    return [true, null, null];
  }

  updateInfo(entity: Partial<BaseCollection<T>>) {
    this.name = entity.name;
    this.updatedAt = entity.updatedAt;
  }

  addItemsToCollection(files: T[]): void {
    if (!files || files.length === 0) return;
    this.items.push(...files);
  }

  removeItemsFromCollection(fileIds: string[]): void {
    if (!fileIds || fileIds.length === 0) return;
    this.items = this.items.filter((x) => !fileIds.some((z) => z === x.fileId));
  }

  abstract isEqualCollectionInfo(content: BaseCollection<T>): boolean;
}
