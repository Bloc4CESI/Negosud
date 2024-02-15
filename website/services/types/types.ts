export type Account = {
  id: number;
  lastName: string;
  firstName: string;
  phoneNumber: string;
  email: string;
  addressId: number;
  address: AddressType;
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
  family: familyType;
  nbProductBox: number;
  dateProduction: Date;
  stock: StockType;
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
  client: Account;
  clientOrderLines: OrderLineType[];
}

export type OrderLineType = {
  id: number;
  quantity: number;
  order: OrderType;
  product: ProductType;
}

export enum OrderStatus {
  REFUSE,
  LIVRE,
  ENCOURSDELIVRAISON,
  VALIDE,
  ENCOURSDEVALIDATION
}

export type StockType = {
    id : number;
    quantity: number;
    minimum: number;
    maximum: number;
    autoOrder: boolean;
    productId: number;
    product : ProductType;
}