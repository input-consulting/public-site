const pages = `
<html>
  <body>
    {% for page in pages %}
    <li>
      <a href="{{ page.route }}">
        {{ page.title }}
      </a>
    </li>
    {% endfor %}
  </body>
</html>
`;

const page = `
<html>
  <body>
    <h1>{{ page.title }}</h1>
    <p>{{ page.body | md }}<p>
  </body>
</html>
`;

module.exports = {
  pages,
  page
};
