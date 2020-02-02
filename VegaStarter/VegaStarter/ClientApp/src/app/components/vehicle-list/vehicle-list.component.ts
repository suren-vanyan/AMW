import { VehicleService } from "./../../services/vehicle.service";
import { Component, OnInit } from "@angular/core";
import { Vehicle } from "src/app/models/vehicle";
import { Observable } from 'rxjs';
import { KeyValuePair } from '../../models/vehicle';

@Component({
  selector: "app-vehicle-list",
  templateUrl: "./vehicle-list.component.html",
  styleUrls: ["./vehicle-list.component.scss"]
})
export class VehicleListComponent implements OnInit {
  vehicles: Vehicle[];
  makes: KeyValuePair[];
  query: any = {
    pageSize:3
  };
  columns=[
    {title:"Id"},
    {title:'Contact Name',key:'contactName',isSortable:true},
    {title:'Make',key:'make',isSortable:true},
    {title:'Model',key:'model',isSortable:true},
]
  constructor(private vehicleService: VehicleService) { }

  ngOnInit() {
    this.vehicleService.getMakes().subscribe(makes => {
      this.makes = makes as KeyValuePair[];
    })
    this.populateVehicles();
  }

  populateVehicles() {
    this.vehicleService.getVehicles(this.query).subscribe(vehicles => {
      this.vehicles = vehicles as Vehicle[];
    })
  }
  onFilterChange() {
    this.query.modelId = 2;
    this.populateVehicles();
  }

  resetFilter() {
    this.query = {};
    this.onFilterChange();
  }


  sortBy(columnName) {
    if (this.query.sortBy === columnName) {
      this.query.isSortAscending = !this.query.isSortAscending;
    }
    else {
      this.query.sortBy = columnName;
      this.query.isSortAscending = true;
    }
    this.populateVehicles();
  }

  onPageChange(page){
    this.query.page=page;
    this.populateVehicles();
  }
}
