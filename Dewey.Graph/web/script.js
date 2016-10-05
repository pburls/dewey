const width = 600;
const height = 400;

const g = new Dracula.Graph

g.addEdge('ExampleWebApiComp', 'ExampleWebApiComp6');

const layouter = new Dracula.Layout.Spring(g);

const renderer = new Dracula.Renderer.Raphael('#canvas', g, width, height);
renderer.draw();
