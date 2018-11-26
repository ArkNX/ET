using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class UILobbyFactory
    {
        public static UI Create()
        {
	        try
	        {
				ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
		        resourcesComponent.LoadBundle(UIType.UILobby.StringToAB());
				GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(UIType.UILobby.StringToAB(), UIType.UILobby);
				GameObject lobby = UnityEngine.Object.Instantiate(bundleGameObject);
				lobby.layer = LayerMask.NameToLayer(LayerNames.UI);
				UI ui = ComponentFactory.Create<UI, GameObject>(lobby, false);

				ui.AddComponent<UILobbyComponent>();
				return ui;
	        }
	        catch (Exception e)
	        {
				Log.Error(e);
		        return null;
	        }
		}

	    public static void Remove(string type)
	    {
			ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(UIType.UILobby.StringToAB());
		}
    }
}