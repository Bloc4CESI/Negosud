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
    <nav className="border-gray-200 bg-gray-900">
      <div className="max-w-screen-xl flex flex-wrap items-center justify-between mx-auto p-4">
        <a href={'/'} className="flex items-center space-x-3 rtl:space-x-reverse">
          <img src={logo.src} className="h-8" alt="NegoSud Logo" />
          <span className="self-center text-2xl font-semibold whitespace-nowrap text-white">NegoSud</span>
        </a>
        <button
          onClick={handleMenuToggle}
          type="button"
          className="inline-flex items-center p-2 w-10 h-10 justify-center text-sm rounded-lg md:hidden hover:bg-gray-700 focus:outline-none focus:ring-2 text-gray-400 focus:ring-gray-600"
          aria-controls="navbar-default"
          aria-expanded={isMenuOpen ? 'true' : 'false'}
        >
         <Burger/>
        </button>
        <div className={`w-full md:block md:w-auto ${isMenuOpen ? 'block' : 'hidden'}`} id="navbar-default">
          <ul className="font-medium flex flex-col p-4 md:p-0 mt-4 rounded-lg bg-gray-900 md:flex-row md:space-x-8 rtl:space-x-reverse md:mt-0">
            <li>
              <a href={`${connected ? '/account' : '/login'}`} className="block py-2 px-3 text-gray-900 rounded hover:bg-gray-500 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0">
                <User className="h-8 w-8 "/>
              </a>
            </li>
            {connected ? (
            <li>
              <a href={"/cart"} className="block py-2 px-3 text-gray-900 rounded hover:bg-gray-500 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0">
                <Cart className="h-8 w-8 "/>
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
