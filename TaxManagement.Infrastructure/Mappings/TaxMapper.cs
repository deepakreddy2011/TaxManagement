using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TaxManagement.Core;

namespace TaxManagement.Infrastructure
{
    public class TaxMapper : Profile
    {
        public TaxMapper()
        {
            CreateMap<MuncipalityTax, MuncipalityTaxDto>()
                .ForMember(dto => dto.Id, domain => domain.MapFrom(tax => tax.Id))
                .ForMember(dto => dto.Muncipality, domain => domain.MapFrom(tax => tax.Muncipality))
                .ForMember(dto => dto.Duration, domain => domain.MapFrom(tax => tax.Duration))
                .ForMember(dto => dto.TaxPriority, domain => domain.MapFrom(tax => tax.TaxPriority))
                .ForMember(dto => dto.StartDate, domain => domain.MapFrom(tax => tax.StartDate))
                .ForMember(dto => dto.EndDate, domain => domain.MapFrom(tax => tax.EndDate))
                .ForMember(dto => dto.TaxRate, domain => domain.MapFrom(tax => tax.TaxRate))
                .ReverseMap();
        }
    }
}
