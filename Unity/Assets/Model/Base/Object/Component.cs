using System;
using MongoDB.Bson.Serialization.Attributes;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace ETModel
{
	[BsonIgnoreExtraElements]
	public abstract class Component : Object, IDisposable
	{
		[BsonIgnore]
		public long InstanceId { get; private set; }

		[BsonIgnore]
		private bool isFromPool;

		[BsonIgnore]
		public bool IsFromPool
		{
			get
			{
				return this.isFromPool;
			}
			set
			{
				this.isFromPool = value;

				if (!this.isFromPool)
				{
					return;
				}

				if (this.InstanceId == 0)
				{
					this.InstanceId = IdGenerater.GenerateId();
#if UNITY_EDITOR
					this.GameObject = ComponentView.Create(this.GetType().Name);
#endif
				}

				Game.EventSystem.Add(this);

			}
		}

		[BsonIgnore]
		public bool IsDisposed
		{
			get
			{
				return this.InstanceId == 0;
			}
		}

		private Component parent;
		
		[BsonIgnore]
		public Component Parent 
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
#if UNITY_EDITOR
				this.GameObject.transform.SetParent(this.parent.GameObject.transform, false);
#endif
			} 
		}

		public T GetParent<T>() where T : Component
		{
			return this.Parent as T;
		}

		[BsonIgnore]
		public Entity Entity
		{
			get
			{
				return this.Parent as Entity;
			}
		}
		
#if UNITY_EDITOR
		GameObject GameObject;
#endif
		
		protected Component()
		{
			this.InstanceId = IdGenerater.GenerateId();
#if UNITY_EDITOR
			this.GameObject = ComponentView.Create(this.GetType().Name);
#endif

		}

		public virtual void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			
#if UNITY_EDITOR
			if (this.GameObject != null)
			{
				UnityEngine.Object.Destroy(this.GameObject);
			}
#endif
			
			// 触发Destroy事件
			Game.EventSystem.Destroy(this);

			Game.EventSystem.Remove(this.InstanceId);
			
			this.InstanceId = 0;

			if (this.IsFromPool)
			{
				Game.ObjectPool.Recycle(this);
			}
		}

		public override void EndInit()
		{
			Game.EventSystem.Deserialize(this);
		}
		
		public override string ToString()
		{
			return MongoHelper.ToJson(this);
		}
	}
}