﻿namespace CsExport.Application.Infrastructure.DependancyControl
{
	public abstract class DependancyConfiguration
	{
		internal void RegisterInternal(IDependancyContainer dependancyContainer)
		{
			Register(dependancyContainer);
		}

		protected abstract void Register(IDependancyContainer dependancyContainer);
	}
}