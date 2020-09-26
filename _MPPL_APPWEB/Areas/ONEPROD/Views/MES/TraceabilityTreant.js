function TraceabilityTreant(gridDivSelector, data)
{
    var self = this;
    var divSelector = gridDivSelector;
    this.chart = {};
    this.RefreshChart = function () {
        //console.log(data);
        InitializeTreantWithData(data);
        this.chart = new Treant(chart_config, function () { console.log('Tree Loaded'); }, $);
    };
    
    function InitializeTreantWithData(dataNodes) {
        chart_config.nodeStructure = dataNodes;
    }
    
    var chart_config = {
        chart: {
            container: divSelector,
            callback: {
                onAfterClickCollapseSwitch: function (nodeSwitch, event) {
                },
                onBeforeClickCollapseSwitch: function (nodeSwitch, event) {
                    console.log(event);
                },
                onCreateNode: function (treeNode, treeNodeDom) {
                    console.log("on create node");
                    treeNodeDom.setAttribute("id", treeNode.id);
                    //$(treeNodeDom).append("<div id=" + treeNode.id + " class='expandNode'> Wartosc </div>");
                }, // this = Tree
                onCreateNodeCollapseSwitch: function (treeNode, treeNodeDom, switchDom) {

                }, // this = Tree
                onAfterAddNode: function (newTreeNode, parentTreeNode, nodeStructure) {

                }, // this = Tree
                onBeforeAddNode: function (parentTreeNode, nodeStructure) {

                }, // this = Tree
                onAfterPositionNode: function (treeNode, nodeDbIndex, containerCenter, treeCenter) {

                }, // this = Tree
                onBeforePositionNode: function (treeNode, nodeDbIndex, containerCenter, treeCenter) {

                }, // this = Tree
                onToggleCollapseFinished: function (treeNode, bIsCollapsed) {

                }, // this = Tree
                onTreeLoaded: function () {
                }// this = Tree
            },
            rootOrientation: 'WEST', // NORTH || EAST || WEST || SOUTH
            // levelSeparation: 30,
            siblingSeparation: 20,
            subTeeSeparation: 60,
            scrollbar: "fancy",
            connectors: {
                type: 'step',
                style: {
                    stroke: 'white'
                }
            },
            node: {
                HTMLclass: 'nodeExample1',
                collapsable: true
            },
            animation: {
                nodeAnimation: "easeInOutCubic",
                nodeSpeed: 700,
            },
        },
        nodeStructure: {},
    };
}