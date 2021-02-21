# DOTS-Entity-Linking
Have you ever facing entity linking problem?
When you need, say, link entity A to your entity B, so you do `EntityManager/EntityCommandBuffer.AddComponent(B, new LinkComponent { value = A })`.
Looks simple, but with your project complexity growing grows need to handle all links, because same entity may be linked with various count of entities by various types of link components.
And soon or not some of your entitits starts to die at random moment of life cycle.
So now you need to care about link validation, need ask `EntityManager.Exists(linkedEntity)` because it can be destroyed already.
Ok, no problem then! I will create system, that will track all my destroyed linked entities and will unlink it from all entities that refers to those.
Well this package can do it for you automatically without creating tons of unlink systems for each concrete type case.

### How it works?
You just take your [**EntityManager**](https://docs.unity3d.com/Packages/com.unity.entities@0.17/manual/ecs_entities.html#creating-entities-with-an-entitymanager) or [**EntityCommandBuffer**](https://docs.unity3d.com/Packages/com.unity.entities@0.17/manual/sync_points.html#avoiding-sync-points) and tell to **Link** or **Unlink** your entities.
Linked entity will store [ISystemStateBufferElement](https://docs.unity3d.com/Packages/com.unity.entities@0.17/manual/system_state_components.html) with all required data for future unlink.
Most important that systems provided by this package will automatically unlink all entities that being destroyed.

### Common use example
EntityManager use case
```csharp
var linkOwnerEntity = EntityManager.CreateEntity();
var linkedEntity = EntityManager.CreateEntity();

//Link is your IComponentData you use for link as usual
//This will automatically get or add LinkDataElement buffer and add link component. All for you :)
//After unlink link component will be reseted
EntityManager.AddLink(linkOwnerEntity, new Link { value = linkedEntity }, linkedEntity);
```
EntityCommandBuffer use case
```csharp
//Create command buffer from any captured EntityCommandBufferSystem
var ecb = _ecbSystem.CreateCommandBuffer();

var linkOwnerEntity = EntityManager.CreateEntity();
var linkedEntity = EntityManager.CreateEntity();

//Link is your IComponentData you use for link as usual
//EntityCommandBuffer is shure that you took care about LinkDataElement buffer adding. Otherwise there will be errors.
//After unlink link component will be reseted
ecb.AddLink(linkOwnerEntity, new Link { value = linkedEntity }, linkedEntity);
```

### Unlink types
* **Removing** - when entities are disconnected component that was used as link component will be removed from link's owner entity.
* **Reseting** (default) - when entities are disconnected component that was used as link component will be reseted (remove and then add new).

### General methods information [DETAILS](MethodsTable.md)
Most of all to do any linking or unlinking there is need to work with some link data. Package use own components. One you can access in LinkDataElement buffer which holds all link data which is required to be able to unlink entities. All this components are stored on linked entity (which one that is linked by link component by other entities, which i called link owners). Some methods will search for those components and add it for you if need. Some components can get buffer from you instead of search it if you care about performance. Some methods assume that linked entity has those components and won't even check it, so in case linked entity has no component you will get accessing errors. Of course you can add components by yourself using EntityManager/EntityCommandBuffer .PrepareEntityForLinking(Entity) with linked entity. Please don't try to add it manually, because some components are internal.

There is three kind of methods:
* **AddLink() / SetLink** - adds link data to LinkDataElement buffer and add/set link component to link's owner entity.
* **Unlink()** - removes link data from LinkDataElement buffer and remove or reset link component on link's owner entity.

But as you can see there is more then just three methods.
Also methods vary by:
* **LinkDataElement buffer accessing**: method can access buffer for you or you can pass buffer to method (to avoid non linear accessing for performance reasons).
* **Link process**: there are actually three Link methods: Link()/AddLink()/SetLink(). First one not add or set any link component, just add data to buffer.
* **Link provider**: you can use EntityManager to link entities immediately or use EntityCommandBuffer to make delayed linking.

### Use recomendations
* You can to not unlink entities by yourself because when linked entity dies clear system will automatically remove all links.
But when this happens automatically clear system can't know does link owner entity still exists, so it will ask `EntityManager.Exists()`.
So too many links can cause performance problems.
* Some of methods assume that linked entity already have LinkDataElement buffer and LinkedEntityTag and won't check it.
So if it's not then you will get accessing error (there is exceptions in code when you are in Debug).
* Please, if you want to use lightweight methods that can take LinkDataElement gotten by you in advance then don't add LinkDataElement buffer manually, instead use `EntityManager/EntityCommandBuffer.PrepareEntityForLinking(Entity)`.
