#region System

global using System.Linq.Expressions;
global using System.Text;

#endregion

#region Microsoft

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;

#endregion

#region NuGets

global using Newtonsoft.Json;
global using RabbitMQ.Client;

#endregion

#region App

global using EventsManager.Application.Interfaces;
global using EventsManager.Domain.Aggregates.EventAggregate;
global using EventsManager.Domain.Aggregates.SpeakerAggregate;
global using EventsManager.Domain.Aggregates.SponsorAggregate;
global using EventsManager.Domain.Main;
global using EventsManager.Infrastructure.Persistance.EF.Contexts;
global using EventsManager.Infrastructure.Persistance.EF.ModelConfigs;

#endregion