"use client"
import React, { useEffect, useState } from "react";
import logo from '../../src/app/images/logo.png';
import Cart from "../../public/img/cart.svg";
import { GiWineBottle } from "react-icons/gi";
import { FiUser } from "react-icons/fi";
import { HiOutlineShoppingCart } from "react-icons/hi";

const Header = ({ style, stroke } : { style?: string, stroke?: string; }) => {
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
    <nav className={`${style} hover:bg-white hover:text-black hover:stroke-black transition-all duration-500 ease-in-out`}>
      <div className="max-w-screen-xl flex items-center justify-between mx-auto p-4 h-16">
        <a href={'/'} className="flex items-center space-x-3 rtl:space-x-reverse">
          <img src={logo.src} className="h-8" alt="NegoSud Logo" />
          <span className="self-center text-2xl font-semibold whitespace-nowrap">NegoSud</span>
        </a>
        <div className="block w-auto" id="navbar-default">
          <ul className="font-medium flex rounded-lg flex-row space-x-8 rtl:space-x-reverse">
            <li>
              <a href={'/listingProducts'}>
              <GiWineBottle className="h-7 w-7" />
              </a>
            </li>
            <li>
              <a href={`${connected ? '/account' : '/login'}`}>
                <FiUser className="h-7 w-7" />
              </a>
            </li>
            {connected ? (
            <li>
              <a href={"/account/cart"}>
                <HiOutlineShoppingCart  className="h-7 w-7"/>
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