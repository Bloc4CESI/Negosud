"use client"
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";

export default function AddressPage() {
  const [connected, setConnected] = useState(false);

  const router = useRouter();
  useEffect(() => {
    if (localStorage.getItem('connected') === null) {
      router.push('/login');
    } else {
      setConnected(true);
    }
  }, []);

  return (
    <>
    <div>
      <h1>Address</h1>
      <div>
        <p>name</p>
        <p>street</p>
        <p>city</p>
        <p>country</p>
      </div>
    </div>
    </>
  )
}