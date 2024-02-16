import React, { useEffect, useState } from "react";
import logo from '../../src/app/images/logo.png';
import { GiWineBottle } from "react-icons/gi";
import { FiUser } from "react-icons/fi";
import { HiOutlineShoppingCart } from "react-icons/hi";
import { getOrderClient } from "../../services/api/products/cart";
import { useAccount } from "../../services/api/user/useAccount";

const Header = ({ style, stroke }: { style?: string, stroke?: string; }) => {
  const [isMenuOpen, setMenuOpen] = useState(false);
  const [connected, setConnected] = useState(false);
  const [ordersLine, setOrderLine] = useState([]);
  const { account } = useAccount();

  console.log(ordersLine);
  useEffect(() => {
    const fetchData = async () => {
      if (localStorage.getItem('connected') !== null) {
        setConnected(true);
        try {
          const ordersData = await getOrderClient(account?.id);
          setOrderLine(ordersData);
        } catch (error) {
          console.error("Erreur lors de la récupération des commandes :", error);
        }
      }
    };
    fetchData();
  }, []);

  const handleMenuToggle = () => {
    setMenuOpen(!isMenuOpen);
  };

  return (
    <nav
      className={`${style} hover:bg-white hover:text-black hover:stroke-black transition-all duration-500 ease-in-out`}>
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
                  <div className="relative">
                    <div className={`t-0 absolute left-3 ${ordersLine.length == 0 ? 'hidden' : ""}`}>
                      <p
                        className="flex h-2 w-2 items-center justify-center rounded-full bg-red-500 p-2 text-xs text-white">{ordersLine?.length}</p>
                    </div>
                    <a href={"/account/cart"}>
                      <HiOutlineShoppingCart className="h-7 w-7" />
                    </a>
                  </div>
                </li>)
              : null}
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default Header;