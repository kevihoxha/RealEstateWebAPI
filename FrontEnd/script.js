document.addEventListener("DOMContentLoaded", function () {
    fetchProperties();
  });
  
  async function fetchProperties() {
    try {
      const response = await fetch("https://localhost:7079/properties"); // Replace with your API endpoint URL
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      const properties = await response.json();
      displayProperties(properties);
    } catch (error) {
      console.error("Error fetching properties:", error);
    }
  }
  async function searchProperties() {
    const searchInput = document.getElementById("searchInput").value;
  
    try {
      const response = await fetch(`https://localhost:7079/properties/search/${searchInput}`);
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      const properties = await response.json();
      displayProperties(properties);
    } catch (error) {
      console.error("Error searching properties:", error);
    }}
    function displayProperties(properties) {
      const propertyList = document.getElementById("propertyList");
      propertyList.innerHTML = ""; // Clear the existing list
  
      properties.forEach((property) => {
          const listItem = document.createElement("li");
          listItem.innerHTML = `
              <h3>${property.title}</h3>
              <p>Location: ${property.location}</p>
              <p>Price: ${property.price}</p>
              <button onclick="openMessageModal(${property.propertyId})">Send Message</button> <!-- New "Send Message" button -->
          `;
  
          propertyList.appendChild(listItem);
      });
  }
    
  function openMessageModal(propertyId) {
    const modal = document.getElementById("messageModal");
    modal.style.display = "block";

    // Store the propertyId in a data attribute of the form, so it can be accessed when the form is submitted
    const messageForm = document.getElementById("messageForm");
    messageForm.setAttribute("data-property-id", propertyId);

    // Add event listener to the form submit event
    messageForm.addEventListener("submit", sendMessage);
}

// Function to close the message modal
function closeModal() {
    const modal = document.getElementById("messageModal");
    modal.style.display = "none";
}

// Function to send the message when the form is submitted
async function sendMessage(event) {
  event.preventDefault();

  const messageForm = document.getElementById("messageForm");
  const propertyId = messageForm.getAttribute("data-property-id");
  const senderName = messageForm.elements["senderName"].value;
  const senderEmail = messageForm.elements["senderEmail"].value;
  const message = messageForm.elements["message"].value;

  try {
      const response = await fetch(`https://localhost:7079/properties/${propertyId}/message`, {
          method: 'POST',
          headers: {
              'Content-Type': 'application/json'
          },
          body: JSON.stringify({
              messageId: 0,
              senderName: senderName,
              senderEmail: senderEmail,
              content: message,
              timestamp: new Date().toISOString(), // Set the current timestamp
              propertyId: 0 // Set propertyId to 0 as requested
          })
      });

      if (!response.ok) {
          throw new Error('Failed to send message');
      }

      // If the request was successful, close the modal
      closeModal();

      // Perform any additional actions as needed, such as showing a success message
  } catch (error) {
      console.error('Error sending message:', error);
      // Handle any errors that occurred during the request
      // For example, show an error message to the user or handle the error gracefully.
  }
}
  
  
  
  
  
  
  
  
  