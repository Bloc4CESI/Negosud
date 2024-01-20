export type Account = {
  lastName: string;
  firstName: string;
  phoneNumber: string;
  email: string;
  addressId: number;
}

export type AddressType = {
  id: number;
  name: string;
  number: number;
  street: string;
  city: string;
  country: string;
}

export type ProductType = {
  id: number;
  name: string;
  price: number;
  description: string;
  image: string;
  home: string;
  familyId: number;
  nbProductBox: number;
  dateProduction: Date;
}