using Camera;
using Gameplay;
using Input;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI
{
    public class LevelLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<JoyStick>().AsSelf();
            builder.RegisterComponentInHierarchy<ClientSystem>().AsSelf();
            builder.RegisterComponentInHierarchy<RestaurantSystem>().AsSelf();
            RegisterPlayer(builder);
            RegisterCamera(builder);
        }

        private void RegisterCamera(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<CameraController>().AsSelf();
        }

        private void RegisterPlayer(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<Player.Player>().AsSelf();
        }
    }
}
