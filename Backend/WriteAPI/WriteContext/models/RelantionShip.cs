﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WriteContext.helpers;

namespace WriteContext.models
{
    public class RelantionShip
    {
        public int FirstUserId { set; get; }
        public User FirstUser { set; get; }

        public int SecondUserId { set; get; }
        public User SecondUser { set; get; }

        public int ActionUserId { set; get; }
        public User ActionUser { set; get; }

        public RelantionShipStatus Status { set; get; }
    }
}
