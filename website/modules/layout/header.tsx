"use client"
import React, { useEffect, useState } from "react";
import logo from '../../src/app/images/logo.png';
import Cart from "../../public/img/cart.svg";
import User from "../../public/img/user.svg";
import Burger from "../../public/img/burger.svg";

const Header = () => {
  const [isMenuOpen, setMenuOpen] = useState(false);
  const [connected, setConnected] = useState(false);

  useEffect(() => {
    if (localStorage.getItem('connected') !== null) {
      setConnected(true);
    }
  }, []);

  const handleMenuToggle = () => {
    setMenuOpen(!isMenuOpen);
  };

  return (
    <nav className="bg-transparent text-white stroke-white hover:bg-white hover:text-black hover:stroke-black transition-all duration-500 ease-in-out">
      <div className="max-w-screen-xl flex items-center justify-between mx-auto p-4 h-16">
        <a href={'/'} className="flex items-center space-x-3 rtl:space-x-reverse">
          <img src={logo.src} className="h-8" alt="NegoSud Logo" />
          <span className="self-center text-2xl font-semibold whitespace-nowrap">NegoSud</span>
        </a>
        <div className="block w-auto" id="navbar-default">
          <ul className="font-medium flex mt-4 rounded-lg flex-row space-x-8 rtl:space-x-reverse">
            <li>
              <a href={`${connected ? '/account' : '/login'}`} className="block py-2 text-gray-900 rounded hover:bg-gray-500 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0">
                <User className="h-8 w-8"/>
              </a>
            </li>
            {connected ? (
            <li>
              <a href={"/cart"} className="block py-2 text-gray-900 rounded hover:bg-gray-500 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0">
                <Cart className="h-8 w-8"/>
              </a>
            </li>)
            : null}
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default Header;