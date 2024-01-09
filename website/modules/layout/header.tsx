import React from "react";
import User from "../../public/img/user.svg";
import Cart from "../../public/img/cart.svg";
import Link from "next/link";

const Header: React.FC = () => {
  return (
    <div className="flex items-center justify-end">
      <h1>NEGOSUD</h1>
      <span className="flex w-12 h-12">
        <Link href="/login" className="flex w-6">
        <User />
        </Link>
        <Cart />
      </span>
    </div>
  );
};

export default Header;
