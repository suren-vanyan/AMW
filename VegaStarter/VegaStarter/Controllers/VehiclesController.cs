﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VegaStarter.Models;
using VegaStarter.Models.Resources;
using VegaStarter.Persistence;

namespace VegaStarter.Controllers
{
    public class VehiclesController : BaseController
    {

        #region Fields
        private readonly IMapper mapper;
        private readonly VegaDbContext dbContext;
        #endregion

        #region Constructor
        public VehiclesController(IMapper mapper, VegaDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }
        #endregion


        #region Actions
        /// <summary>
        /// Create new vehicle
        /// </summary>
        /// <param name="vehicleResource"></param>
        /// <returns>VehicleResource</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateVehicle([FromBody]VehicleResource vehicleResource)
        {

            var vehicle = mapper.Map<VehicleResource, Vehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;
            dbContext.Set<Vehicle>().Add(vehicle);
            int x = await dbContext.SaveChangesAsync().ConfigureAwait(false);
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(result);
        }

        /// <summary>
        /// Update Vehicle
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vehicleResource"></param>
        /// <returns>VehicleResource</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody]VehicleResource vehicleResource)
        {
            //Find Vehicle
            var vehicle = await dbContext.Vehicles.Include(v => v.VehicleFeatures).SingleOrDefaultAsync(v => v.Id == id).ConfigureAwait(false);

            if (vehicle == null)
                return new NotFoundObjectResult(vehicleResource);

            mapper.Map(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;

            await dbContext.SaveChangesAsync().ConfigureAwait(false);
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await dbContext.Vehicles.FindAsync(id).ConfigureAwait(false);

            if (vehicle == null)
                return new NotFoundObjectResult(id);

            dbContext.Remove<Vehicle>(vehicle);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await dbContext.Vehicles.Include(v => v.VehicleFeatures).SingleOrDefaultAsync(v => v.Id == id).ConfigureAwait(false);

            if (vehicle == null)
                return new NotFoundObjectResult(id);

            return Ok(vehicle);
        }

        #endregion
    }
}