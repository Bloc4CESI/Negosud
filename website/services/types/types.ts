import { DateTime } from "next-auth/providers/kakao";

export type Account = {
  lastName: string;
  firstName: string;
  phoneNumber: string;
  email: string;
  addressId: number;
}

export type ProductType = {
  id : number;
  name: string;
  price: number;
  image: string;
  description: string;
  dateProduction: DateTime;
  nbProductBox: number;
  familyId: number;
  family: familyType;
  home: string;
};

export type familyType = {
  id : number;
  name: string;
};