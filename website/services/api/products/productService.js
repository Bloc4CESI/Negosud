import toast from "react-hot-toast";

const API_BASE_URL = 'https://localhost:7281/api';
export async function getProducts(sortOrder = '') {
  // Construire l'URL avec le paramètre de tri si spécifié
  const url = new URL(`${API_BASE_URL}/Product`);
  if (sortOrder) {
    url.searchParams.append('sortOrder', sortOrder);
  }
  const response = await fetch(url, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  if (!response.ok) {
    throw new Error('Erreur lors de la récupération des produits');
  }

  return await response.json();
}


export async function getFamily() {
  const response = await fetch(`${API_BASE_URL}/Family`, {

    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });
  if (!response.ok) {
    throw new Error('Erreur lors de la récupération des familles');
  }

  return await response.json();
}
  
  export async function getProductByID(id) {
    const response = await fetch(`${API_BASE_URL}/Product/${id}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    if (!response.ok) {
      throw new Error('Erreur lors de la récupération des produits');
    }

    return await response.json();
  }

export async function getProductByName(data) {
  const response = await fetch(`${API_BASE_URL}/Product/?Name=${data}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });
  if (!response.ok) {
    toast.error('Aucun produits dans la séléction');
  }

  return await response.json();
}
    
export async function getFamilyById(id, sortOrder = '') {
  const url = new URL(`${API_BASE_URL}/Product/GetProductByFamily/${id}`);
  if (sortOrder) {
    url.searchParams.append('sortOrder', sortOrder);
  }

  const response = await fetch(url, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  if (!response.ok) {
    throw new Error('Erreur lors de la récupération de la famille');
  }

  return await response.json();
}