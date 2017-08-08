const app = require('./server/app');

app.listen(process.env.SERVICE_PORT || 8000);