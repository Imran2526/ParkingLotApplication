﻿using ParkingLotModelLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotRepositoryLayer.IRepository
{
    public interface IParkingRepository
    {
        ParkingModel ParkingVehical(ParkingModel park);

        ParkingModel UnparkingVehical(ParkingModel unpark);

        ParkingModel SearchVehical(ParkingModel search);

        ParkingModel DeleteUnparkVehical(ParkingModel delete);

    }
}
