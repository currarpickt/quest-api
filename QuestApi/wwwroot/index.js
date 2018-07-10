// JS code to call the API endpoints
// call `api/progress` with POST
const postProgress = () => {
  const apiUrl = document.getElementById('apiUrl').value;
  const dataParam = document.getElementById('dataParam').value;

  fetch(`${apiUrl}/api/progress`, {
    method: 'POST',
    headers: {
      "Content-type": "application/json"
    },
    body: dataParam,
  }).then(response => {
    document.getElementById('progressStatus').innerHTML = response.status;
    if (response.status !== 200) {
      console.error(response.status);
      return;
    }
    response.json().then(data => {
      document.getElementById('progressResponse').value = JSON.stringify(data, null, 2);
    });
  }).catch(err => {
    console.error(err);
  });
};

// call `api/state` with GET
const getState = () => {
  const apiUrl = document.getElementById('apiUrl').value;
  const playerId = document.getElementById('playerId').value;

  fetch(`${apiUrl}/api/state/${playerId}`).then(response => {
    console.log(response);
    document.getElementById('stateStatus').innerHTML = response.status;
    if (response.status !== 200) {
      console.error(response.status);
      return;
    }
    response.json().then(data => {
      document.getElementById('stateResponse').value = JSON.stringify(data, null, 2);
    });
  }).catch(err => {
    console.error(err);
  });
};

// populate fields with sample data on load
(() => {
  const playerId = 'P01';
  const sampleDataParam = {
    PlayerId: playerId,
    PlayerLevel: 1,
    ChipAmountBet: 10
  };
  document.getElementById('playerId').value = playerId;
  document.getElementById('dataParam').value = JSON.stringify(sampleDataParam, null, 2);
})();
