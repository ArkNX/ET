using ETModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
#if !ILRuntime
	public abstract class Component: ETModel.Component
	{
		[BsonIgnore]
		public new Component Parent
		{
			get
			{
				return (Component) base.Parent;
			}
			set
			{
				base.Parent = value;
			}
		}
		
		public new T GetParent<T>() where T : Component
		{
			return this.Parent as T;
		}

		[BsonIgnore]
		public new Entity Entity
		{
			get
			{
				return this.Parent as Entity;
			}
		}
	}
#else
	[BsonIgnoreExtraElements]
	public abstract class Component : Object, IDisposable
	{
		[BsonIgnore]
		public long InstanceId { get; protected set; }

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

				this.InstanceId = IdGenerater.GenerateId();
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

		[BsonIgnore]
		public Component Parent { get; set; }

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

		protected Component()
		{
			this.InstanceId = IdGenerater.GenerateId();
		}
		
		public virtual void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			
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
	}
#endif
}