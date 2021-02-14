﻿using ParkingLotModelLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotRepositoryLayer.IRepository
{
    public interface IParkingRepository
    {
        ParkingModel ParkingVehical(ParkingModel park);

        ParkingResponse UnparkingVehical(int slotNo);

        bool DeleteVehicals();

        IEnumerable<ParkingModel> SearchVehical(string vehicalNo);

        IEnumerable<ParkingModel> SearchVehical(int slotNo);
    }
}
