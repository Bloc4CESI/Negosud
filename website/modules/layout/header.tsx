import Image from 'next/image'
import img from "../../src/app/images/logo.png"
import Link from 'next/link';

const Header = () => {

  return (
    <header className="bg-dark-bckground sticky top-0 z-[20] mx-auto flew w-full items-center justify-between border-b border-gray-500 p-8 flex">
      <Image src={img} width={64} height={64} alt={""}/>
      <Link href={'/login'} >
      <button>Connectez-vous</button>
      </Link>
    </header>
  );
};

export default Header;
