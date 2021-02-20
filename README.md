# DOTS-Entity-Linking
Have you ever facing entity linking problem?
When you need, say, link entity A to your entity B, so you do `EntityManager/EntityCommandBuffer.AddComponent(B, new LinkComponent { value = A })`.
Looks simple, but with your project complexity growing grows need to handle all links, because same entity may be linked with various count of entities by various types of link components.
And soon or not some of your entitits starts to die at random moment of life cycle.
So now you need to care about link validation, need ask `EntityManager.Exists(linked Entity)` because it can be destroyed already.
Ok, no problem then! I will create system, that will track all my destroyed linked entities and will unlink it from all entities that refers to those.
Well this package can do it for you automatically without creating tons of unlink systems for each concrete type case.

### How it works?
You just take your [**EntityManager**](https://docs.unity3d.com/Packages/com.unity.entities@0.17/manual/ecs_entities.html#creating-entities-with-an-entitymanager) or [**EntityCommandBuffer**](https://docs.unity3d.com/Packages/com.unity.entities@0.17/manual/sync_points.html#avoiding-sync-points) and tell to **Link** or **Unlink** your entities.
Linked entity will store [ISystemStateBufferElement](https://docs.unity3d.com/Packages/com.unity.entities@0.17/manual/system_state_components.html) with all required data for future unlink.
Most important that systems provided by this package will automatically unlink all entities that being destroyed.

### Common use example
EntityManager use case
```csharp
var linkOnwerEntity = EntityManager.CreateEntity();
var linkedEntity = EntityManager.CreateEntity();

//Link is your IComponentData you use for link as usual
//This will automatically get or add LinkDataElement buffer and add link component. All for you :)
//After unlink link component will be reseted
EntityManager.Link(linkOwnerEntity, new Link { value = linkedEntity }, linkedEntity);
```
EntityCommandBuffer use case
```csharp
//Create command buffer from any captured EntityCommandBufferSystem
var ecb = _ecbSystem.CreateCommandBuffer();

var linkOnwerEntity = EntityManager.CreateEntity();
var linkedEntity = EntityManager.CreateEntity();

//Link is your IComponentData you use for link as usual
//EntityCommandBuffer is shure that you took care about LinkDataElement buffer adding. Otherwise there will be errors.
//After unlink link component will be reseted
ecb.Link(linkOwnerEntity, new Link { value = linkedEntity }, linkedEntity);
```

### Unlink types
* **Removing** - when entities unlinks component that was used as link component will be removed from link's owner entity.
* **Reseting** (default) - when entities unlinks component that was used as link component will be reset (remove and then add new).

### Methods
In general there is two kind of methods:
* **Link()** - adds link data to LinkDataElement buffer and link component to link's owner entity.
* **Unlink()** - removes link data from LinkDataElement buffer and remove or reset link component on link's owner entity.

But as you can see there is more then just two methods.
Also methods vary by:
* **LinkDataElement buffer accessing**: method can access it for you or you can pass to methods (to avoid non linear accessing for performance reasons).
* **Link process**: there are actually two Link methods: Link() and RegistrateLink(). Second one not add any link component, just add data to buffer.
* **Link provider**: you can use EntityManager to link entities just now or use EntityCommandBuffer to make delayed addition.

### Use recomendations
* You can to not unlink entities by yourself because when linked entity dies clear system will automatically remove all links.
But when this happens automatically system can't know do link owner entity still exists, so it will ask `EntityManager.Exists()`.
So too many links can cause performance problems.
* Some of methods assume that linked entity already have LinkDataElement buffer and won't check it.
So if it's not then you will get accessing error (there is exceptions in code when you are in Debug).
