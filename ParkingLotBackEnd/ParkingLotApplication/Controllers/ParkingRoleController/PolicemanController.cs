﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ParkingLotBusinessLayer.IBusinessLayer;
using ParkingLotBusinessLayer.IParkingBusinessLayer;
using ParkingLotModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Policeman")]
    public class PolicemanController : ControllerBase
    {

        private readonly IParkingBusiness policeParking;
        private IDistributedCache cache;
        private string cacheKey;
        private DistributedCacheEntryOptions options;

        public PolicemanController(IParkingBusiness policeParking, IDistributedCache cache)
        {
            this.policeParking = policeParking;
            this.cache = cache;
            this.cacheKey = "parkingLot";
            this.options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(3.0));
        }

        /// <summary>
        /// Policeman Vehical Parking.
        /// </summary>
        /// <param name="park">The park.</param>
        /// <returns></returns>

        [HttpPost]
        public IActionResult PolicemanVehicalPark([FromBody] ParkingModel park)
        {
            try
            {
                var result = this.policeParking.ParkingVehical(park);
                if (result != null && park.DriverTypeID==3)
                {
                    return this.Ok(new { Status = true, Message = "Parking Succesfully", Data = result });
                }
                    return this.BadRequest(new { Status = false, Message = "Error While Parking"});
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        /// <summary>
        /// Unpark Vehical if the entry is true.
        /// </summary>
        /// <param name="unpark">The unpark.</param>
        /// <returns></returns>

        [HttpPut]
        public IActionResult PolicemanVehicalUnpark([FromQuery] int parkingId)
        {
            try
            {
                var unparks = this.policeParking.UnparkingVehical(parkingId);

                if (unparks.IsEmpty == false)
                {
                    return this.Ok(new { Status = true, Message = "Park", Data = unparks });
                }
                if (unparks.IsEmpty == true)
                {
                    return this.Ok(new { Status = true, Message = "UnPark", Data = unparks });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Error While Unparking" });
                }
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        /// <summary>
        /// Delete the vehical if its False.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        
        [HttpDelete]
        public IActionResult DeleteVehical()
        {
            try
            {
                bool delete = this.policeParking.DeleteVehicals();

                if (delete)
                {
                    return this.Ok(new { Status = true, Message = "Empty Vehicale Slots Deleted Sucess", Data = delete });
                }
                    return this.BadRequest(new { Status = false, Message = "Error While Deleting" });
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        /// <summary>
        /// Searches the vehical by slotNo an Vehical No.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns></returns>

        [HttpGet]
        [Route("searchVehical")]
        public IActionResult SearchVehical(int slotNo, string vehicalNo)
        {
            try
            {
                IEnumerable<ParkingModel> searchResult = this.policeParking.SearchVehical(slotNo, vehicalNo);

                if (searchResult != null)
                {
                    this.cache.SetString(this.cacheKey, JsonConvert.SerializeObject(searchResult));
                }

                ///Redis Cashe Implemented
                if (this.cache.GetString(this.cacheKey) != null)
                {
                    var data = JsonConvert.DeserializeObject<List<ParkingModel>>(this.cache.GetString(this.cacheKey));
                    return this.Ok(new { Status = true, Message = "Park Vehical Data Retrive Succesfully", Data = data });
                }
                return this.NotFound(new { Status = true, Message = "Data Not Found", Data = searchResult });
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }

        /// <summary>
        /// Gets all park vehical.
        /// </summary>
        /// <param name="IsEmpty">if set to <c>true</c> [is empty].</param>
        /// <returns></returns>

        [HttpGet]
        public IActionResult GetAllParkVehical()
        {
            try
            {
                IEnumerable<ParkingModel> getResult = this.policeParking.GetParkVehicalData();

                if (getResult != null)
                {
                    this.cache.SetString(this.cacheKey, JsonConvert.SerializeObject(getResult));
                }

                ///Redis Cashe Implemented
                if (this.cache.GetString(this.cacheKey) != null)
                {
                    var data = JsonConvert.DeserializeObject<List<ParkingModel>>(this.cache.GetString(this.cacheKey));
                    return this.Ok(new { Status = true, Message = "Park Vehical Data Retrive Succesfully", Data = data });
                }
                    return this.NotFound(new { Status = false, Message = "Data Not Found", Data = getResult });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = e.Message });
            }
        }
    }
}
