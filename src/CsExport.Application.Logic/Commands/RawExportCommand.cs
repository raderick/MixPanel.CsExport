﻿using System;
using CsExport.Application.Infrastructure;
using CsExport.Application.Infrastructure.Results;
using CsExport.Application.Logic.CommandArguments;
using CsExport.Application.Logic.Results;
using CsExport.Core.Client;
using CsExport.Core.Settings;

namespace CsExport.Application.Logic.Commands
{
	public class RawExportCommand : ICommandWithArguments<RawExportCommandArguments>
	{
		private readonly ClientConfiguration _clientConfiguration;
		private readonly IMixPanelClient _mixPanelClient;
		private readonly IFileWriter _fileWriter;

		public RawExportCommand(ClientConfiguration clientConfiguration,
		                        IMixPanelClient mixPanelClient,
		                        IFileWriter fileWriter)
		{
			_clientConfiguration = clientConfiguration;
			_mixPanelClient = mixPanelClient;
			_fileWriter = fileWriter;
		}

		public CommandResult Execute(RawExportCommandArguments arguments)
		{
			if (arguments == null)
				throw new ArgumentNullException(nameof(arguments));

			if (arguments.From.GetDateTime() > arguments.To.GetDateTime())
				throw new ArgumentException("@from should represent a date less than @to", nameof(arguments.From));

			if (_clientConfiguration == null
			    || string.IsNullOrWhiteSpace(_clientConfiguration.Secret))
				return new UnauthorizedResult();

			var content = _mixPanelClient.ExportRaw(_clientConfiguration, arguments.From, arguments.To, arguments.Events);

			_fileWriter.WriteContent(_clientConfiguration.ExportPath,
			                         $"{DateTime.Now.ToFileTimeUtc()}-raw-export.txt",
			                         content);

			return new SuccessResult();
		}
	}
}