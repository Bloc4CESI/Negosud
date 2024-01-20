import { AddressType } from "../../services/types/types";


export default function Address({addresses }: {addresses: AddressType[]}) {
return (
  <div>
    {Array.isArray(addresses) && addresses.length > 0 ? (
      addresses.length === 1 ? (
        // Cas où il y a une seule adresse
        <div key={addresses[0].id}>
          <p>{addresses[0].name}</p>
          <p>{addresses[0].number} {addresses[0].street}</p>
          <p>{addresses[0].city}</p>
          <p>{addresses[0].country}</p>
        </div>
      ) : (
        // Cas où il y a plusieurs adresses
        addresses.map((address) => (
          <div key={address.id}>
            <p>{address.name}</p>
            <p>{address.number} {address.street}</p>
            <p>{address.city}</p>
            <p>{address.country}</p>
          </div>
        ))
      )
    ) : (
      // Cas où il n'y a pas d'adresse
      <p>Vous n'avez pas d'adresse.</p>
    )}
  </div>
);
}