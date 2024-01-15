"use client"

export default function User({ userData }: { userData: any }) {

  return (
    <div>
      Bienvenue
      {userData.FirstName}
      {userData.LastName}
    </div>
  )
}

