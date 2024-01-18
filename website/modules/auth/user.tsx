"use client"

export default function User({ userData }: { userData: any }) {

  return (
    <div>
      Bienvenue
      {userData.firstName}
      {userData.lastName}
      {userData.email}
    </div>
  )
}

