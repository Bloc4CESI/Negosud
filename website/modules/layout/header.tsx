// use client;
import React from "react";
import { Link } from "react-router-dom";  // Ajout de l'import pour Link
import User from "../../public/img/user.svg";
import Cart from "../../public/img/cart.svg";

const Header: React.FC = () => {
  return (
    <div className="flex items-center justify-end">
      <h1>NEGOSUD</h1>
      <span className="flex w-12 h-12">
        <Link to="/login">  {/* Utilisation de Link pour la navigation */}
          <User />
        </Link>
        <Cart />
      </span>
    </div>
  );
};

export default Header;
