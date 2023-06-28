#region System

global using System.Linq.Expressions;

#endregion

#region Microsoft

global using Microsoft.Extensions.Logging;

#endregion

#region NuGets

global using AutoMapper;
global using FluentValidation;

#endregion

#region App

global using EventsManager.Application.DTOs;
global using EventsManager.Application.Exceptions;
global using EventsManager.Application.Interfaces;
global using EventsManager.Application.Main;
global using EventsManager.Domain.Aggregates.EventAggregate;
global using EventsManager.Domain.Aggregates.SpeakerAggregate;
global using EventsManager.Domain.Aggregates.SponsorAggregate;
global using EventsManager.Domain.Main;

#endregion