export type Account = {
  id: number;
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

export type familyType = {
  id : number;
  name: string;
};

export type OrderType = {
  id: number;
  date: string;
  price: number;
  orderStatus: OrderStatus;
  clientId: number;
}

export type OrderLineType = {
  id: number;
  quantity: number;
  price: number;
  orderId: number;
  productId: number;
}

export enum OrderStatus {
  REFUSE,
  LIVRE,
  ENCOURSDELIVRAISON,
  VALIDE,
  ENCOURSDEVALIDATION
}