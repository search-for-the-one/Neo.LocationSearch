# Neo.LocationSearch

Neo location search is one library which you can use it to search nearby suburbs by geo location and distance.

## How does it work

Neo location search get suburb boundary data from `IBoundaryDataProvider` and geo location data from `ILocationDataProvider` to build geo map. Geo map cuts the map area into two dimension array evenly with resolution, then it can map one geo location into one element in the two dimension array and then do nearby suburb search.

## Boundary data

In this repo, there is one data source - australia open data https://data.gov.au/search?q=boundaries

## Location data

If you cannot get boundary data, you can use location data provider. You need to find some data source to get suburb and geo location within the suburb.

## How to use this project

In the GeoMapGenerator project appsettings, you can config your options. Run the GeoMapGenerator project and it will get data from boundary data provider and location data provider, and build geo map. After build is successful, it will dump geo map to file and you can use the data file next time.

## Use data file

Add DI binding `services.AddSingleton<IGeoMapConstructor, GeoMapConstructor>();` Use `FromJsonFile` or `FromBinaryFile` to create geo map.
