﻿using System;
using CsExport.Application.Logic.Commands;
using CsExport.Application.Logic.Results;
using CsExport.Core;
using CsExport.Core.Settings;
using Moq;
using Xunit;

namespace CsExport.Application.Logic.Tests.CommandTests
{
	public class RawExportCommandTests : CommandTestsBase
	{  
		[Fact]
		public void Execute_When_called_with_valid_parameters_Then_returns_correct_result_type()
		{
			var command = GetCommand();

			var result = command.Execute(GetExecutionSettings());

			Assert.IsType<SuccessResult>(result);
		}

		[Fact]
		public void Execute_When_called_with_valid_parameters_Then_calls_mixPanelClient()
		{
			var command = GetCommand();

			var result = command.Execute(GetExecutionSettings());

			MixPanelClientMock.Verify(x=>x.ExportRaw(ClientConfiguration, It.IsAny<Date>(), It.IsAny<Date>()), Times.Once);
		}

		[Fact]
		public void Execute_When_called_with_valid_parameters_Then_passes_these_parameters_into_mixPanelClient()
		{
			var @from = new Date(2010, 10, 31);
			var to = new Date(2011, 1, 1);

			var command = GetCommand(@from, to);

			command.Execute(GetExecutionSettings());
			
			MixPanelClientMock.Verify(x => x.ExportRaw(ClientConfiguration, @from, to), Times.Once);
		}

		[Fact]
		public void Ctor_When_called_with_wrong_date_range_Then_throws_argumentException()
		{
			var @from = new Date(2011, 1, 1);
			var to = new Date(2010, 10, 31);

			Assert.Throws<ArgumentException>(() => GetCommand(from, to));
		}

		private RawExportCommand GetCommand(Date from, Date to)
		{
			return new RawExportCommand(from, to);
		}	 

		private RawExportCommand GetCommand()
		{
			var @from = new Date(2011, 1, 1);
			var to = new Date(2011, 12, 31);

			return new RawExportCommand(from, to);
		}
	}
}