using AppCore.Entities;
using AppCore.Enums;

namespace AppCore.Dto;

public record AddressDto(
    string Street,
    string City,
    string PostalCode,
    string Country,
    AddressType Type
)
{
    public static AddressDto? FromEntity(Address? address)
    {
        if (address is null)
            return null;

        return new AddressDto(
            address.Street,
            address.City,
            address.PostalCode,
            address.Country,
            address.Type
        );
    }

    public static Address? ToEntity(AddressDto? dto)
    {
        if (dto is null)
            return null;

        return new Address
        {
            Street = dto.Street,
            City = dto.City,
            PostalCode = dto.PostalCode,
            Country = dto.Country,
            Type = dto.Type
        };
    }
}