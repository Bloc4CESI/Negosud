import bcrypt from "bcryptjs";

export const API_BASE_URL = 'https://localhost:7281/api';

export async function Logout() {
  localStorage.removeItem('connected');
  localStorage.removeItem('account');
}

export async function createUser(data) {
  const response = await fetch(`${API_BASE_URL}/Client/PostWithAddress`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    throw new Error('Erreur lors de la création de l\'utilisateur');
  }

    return await response.text();
}

export async function createAddress(data, userData) {
  const response = await fetch(`${API_BASE_URL}/Address`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    throw new Error('Erreur lors de la création de l\'adresse');
  }

  const addressResponse = await fetch(`${API_BASE_URL}/Address?Name=${data.name}&City=${data.city}&Country=${data.country}&Street=${data.street}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  if (!addressResponse.ok){
    throw new Error('Erreur lors de la récupération de l\'adresse');
  }

  const addressData = await addressResponse.json();
  const firstAddress = addressData[0];
  const addressId = firstAddress.id
  const clientId = userData.id;
  userData.addressId = addressId
  userData.address = firstAddress;
  userData.address.client = userData;
  console.log(userData);
  const addAddressToClient = await fetch(`${API_BASE_URL}/Client/${clientId}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(userData),

  });
  if (!addAddressToClient.ok) {
    throw new Error('Erreur lors de l\'ajout de l\'adresse à l\'utilisateur');
  }
  localStorage.setItem('account', JSON.stringify(userData));
  return await addAddressToClient.text();
}

export async function getUser(email, password) {
  const encodedEmail = encodeURIComponent(email);
    const response = await fetch(`${API_BASE_URL}/Client/GetByEmail/${encodedEmail}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });

  if (!response.ok) {
    throw new Error('Erreur lors de la récupération de l\'utilisateur');
  }

  const userData = await response.json();

  const connect = bcrypt.compare(password, userData.password).then((match) => {
    return !match;
  });

  return userData;
}

export async function getAddresses(id) {
  const response = await fetch(`${API_BASE_URL}/Address/${id}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  if (!response.ok) {
    throw new Error('Erreur lors de la récupération des adresses');
  }
  return await response.json();
}

export async function getOrders(id) {
  const response = await fetch(`${API_BASE_URL}/ClientOrder/GetOrdersByClient/${id}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  if (!response.ok) {
    throw new Error('Erreur lors de la récupération des commandes');
  }
  return await response.json();
}

export async function modifyAddressClient(data, id){
  const response = await fetch(`${API_BASE_URL}/Address/${id}`,{
    method: 'PUT',
    headers : {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    throw new Error('Erreur lors de la mise à jour de la commande');
  }
  return await response.text();
}