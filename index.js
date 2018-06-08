require("./server/app")
  .startup()
  .then(app => app.listen(process.env.PORT || 3000))
  .catch(err => console.error(err));
