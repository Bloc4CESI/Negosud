const API_BASE_URL = "https://localhost:7281/api";

export async function getOrderClient(Id) {
  const response = await fetch(`${API_BASE_URL}/ClientOrderLine/GetClientCart/${Id}`,{
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  if (!response.ok) {
    throw new Error('Erreur lors de la récupération des lignes commande');
  }

  return await response.json();
}

export async function postOrderClientLine(productId, clientId,quantity,price) {
    const response = await fetch(`${API_BASE_URL}/ClientOrderLine/AddProductToCart?ProductId=${productId}&ClientId=${clientId}&Quantity=${quantity}&Price=${price}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
    });
  
    if (!response.ok) {
      throw new Error('Erreur lors de la création de la ligne de commande');
    }
  
      return await response.text();
}

export async function deleteOrderClientLine(Id) {
  const response = await fetch(`${API_BASE_URL}/ClientOrderLine/RemoveProductFromCart/${Id}`,{
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  if (!response.ok) {
    throw new Error('Erreur lors de la suppression de la ligne de commande');
  }

    return await response.text();
}

export async function getOrderById(id){
  const response = await fetch(`${API_BASE_URL}/ClientOrder/GetOrderClientById/${id}`,{
    method: 'GET',
    headers : {
      'Content-Type': 'application/json',
    },
  });
  if (!response.ok) {
    throw new Error('Erreur lors de la recuperation de la commande');
  }
  return await response.json();
}

export async function putOrderClient(data){
  const response = await fetch(`${API_BASE_URL}/ClientOrder/UpdateOrder`,{
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