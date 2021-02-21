# Methods table
EntityManager
Overloading | Auto component accessing | Description
------------ | ------------- | -------------
`Link<TLink>(Entity, DynamicBuffer<LinkDataElement>, [bool])` | :x: | Simply add link data to LinkDataElement buffer
`Link<TLink>(Entity, Entity, [bool])` | :heavy_check_mark: | Simply add link data to LinkDataElement buffer
`AddLink<TLink>(Entity, T, DynamicBuffer<LinkDataElement>, [bool])` | :x: | Add TLink component to link owner and add link data to LinkDataElement buffer
`AddLink<TLink>(Entity, T, Entity, [bool])` | :heavy_check_mark: | Add TLink component to link owner and add link data to LinkDataElement buffer
`SetLink<TLink>(Entity, T, DynamicBuffer<LinkDataElement>, [bool])` | :x: | Set TLink component to link owner and add link data to LinkDataElement buffer
`SetLink<TLink>(Entity, T, Entity, [bool])` | :heavy_check_mark: | Set TLink component to link owner and add link data to LinkDataElement buffer
`UnLink<TLink>(DynamicBuffer<LinkDataElement>, Entity, Entity)` | :x: | Unlink (removes or resets link component on link owner depending on [bool]) entities and removes link data from buffer
`UnLink<TLink>(Entity, Entity)` | :heavy_check_mark: | Unlink (removes or resets link component on link owner depending on [bool]) entities and removes link data from buffer
`UnLink<TLink>(LinkDataElement)` | - | Simply removes or resets link component on link owner depending on [bool] without any LinkDataBuffer changes

EntityCommandBuffer (all links changes will be done delayed)
Overloading | Auto component accessing | Description
------------ | ------------- | -------------
`Link<TLink>(Entity, Entity, [bool])` | :x: | Simply add link data to LinkDataElement buffer
`AddLink<TLink>(Entity, TLink, Entity, [bool])` | :x: | Simply add link data to LinkDataElement buffer
`SetLink<TLink>(Entity, TLink, Entity, [bool])` | :x: | Set TLink component to link owner and add link data to LinkDataElement buffer
`UnLink<TLink>(DynamicBuffer<LinkDataElement>, Entity, Entity)` | :x: | Unlink (removes or resets link component on link owner depending on [bool]) entities and removes link data from buffer
`UnLink<TLink>(SystemBase, Entity, Entity)` | :heavy_check_mark: | Unlink (removes or resets link component on link owner depending on [bool]) entities and removes link data from buffer
`UnLink<TLink>(LinkDataElement)` | - | Simply removes or resets link component on link owner depending on [bool] without any LinkDataBuffer changes
