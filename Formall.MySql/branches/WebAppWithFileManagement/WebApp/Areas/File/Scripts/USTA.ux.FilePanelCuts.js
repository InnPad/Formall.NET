
/**
* Event handler for when the thumbnail panel is rendered
* Configures dragzone for dataview
*
* @private
* @param	{Ext.DataView} dataView	The DataView from the panel
* @returns	{Void}
*/
onThumbRender: function (dataView) {
    // configure new DragZone object
    dataView.dragZone = Ext Ext.dd.DragZone(dataView.getEl(), {
        /**
        * DragDrop group this zone belongs to
        */
        ddGroup: 'fileMoveDD',

        /**
        * Implementation of getDragData
        * Returns an object literal containing information about what is being dragged
        *
        * @param	{Ext.EventObject} evt	MouseDown event
        * @returns	{Object}				Object with drag data
        */
        getDragData: function (evt) {
            var sourceEl, dragProxyHtml;
            // get the event target, that be the item that is beiing dragged
            sourceEl = evt.getTarget(dataView.itemSelector);
            if (sourceEl) {
                // if nothing is selected, select the node beiing dragged
                // else/if multiple nodes are selected and the node beiing dragged is outside the selection,
                // select the node beiing dragged
                if (dataView.getSelectedNodes().length === 0) {
                    dataView.select(sourceEl);
                } else if (!evt.ctrlKey && !evt.shiftKey &&
							  dataView.getSelectedNodes().length > 0 &&
							  dataView.getSelectedNodes().indexOf(sourceEl) === -1) {
                    dataView.select(sourceEl);
                }

                // clone the node to use as dragProxy
                dragProxyHtml = sourceEl.cloneNode(true);
                dragProxyHtml.id = Ext.id();

                // compose and return dragData
                return {
                    ddel: dragProxyHtml,
                    sourceEl: sourceEl,
                    repairXY: Ext.fly(sourceEl).getXY(),
                    sourceStore: dataView.store,
                    draggedRecord: dataView.getRecord(sourceEl),
                    selections: dataView.getSelectedRecords()
                };
            }

            return null;
        },

        /**
        * Gets the XY position used to restore the drag in case of invalid drop
        *
        * @returns	{Array}	XY location
        */
        getRepairXY: function () {
            return this.dragData.repairXY;
        }
    });
}, // onThumbRender