
/**
* @class		Ext.ux.ThumbsPanel
* @extends		Ext.Panel
* @namespace	Ext.ux
*
* UploadPanel
*
* @author		Lucas M. Oromi
* @version		0.1.1
*
* @licence		GNU General Public License v3
* 				http://www.gnu.org/licenses/lgpl.html
*/

// Create namespace
Ext.namespace('Ext.ux');

/**
* Create new Extension
*
* @constructor
* @param {Object} config	An object literal config object
*/
Ext.define('Ext.ux.ThumbsPanel', {
    extend: 'Ext.Panel',
    //cls: 'ux-file-thumbs',
    layout: 'fit',
    border: false,
    /**
    * @cfg {String} dataUrl The URL that is used to process data (required)
    */
    store: null,
    view: 'large',

    /**
    * Called by Ext when instantiating
    *
    * @private
    * @param {Object} config Configuration object
    */
    initComponent: function () {
        var config;

        var dataView = new Ext.DataView({
            store: this.store,
            tpl: new Ext.XTemplate(
				'<tpl for=".">',
					'<div class="ux-file-thumb-wrap" id="ux-file-thumb-{#}">',
						'<div class="ux-file-thumb"><img src="{Thumbnail}" title="{Name}"></div>',
						'<span class="x-editable">{Name:ellipsis(18)}</span>',
					'</div>',
				'</tpl>',
				'<div class="x-clear"></div>'),
            style: {
                overflow: 'auto'
            },
            multiSelect: false,
            trackOver: true,
            overItemCls: 'x-item-over',
            overClass: 'x-view-over',
            itemSelector: '.ux-file-thumb-wrap',
            emptyText: '<div class="x-grid-empty">' + this.il8n.noFilesText + '</div>',
            plugins: [
                Ext.create('Ext.ux.DataView.DragSelector', {}),
                Ext.create('Ext.ux.DataView.LabelEditor', {
                    dataIndex: 'Name',
                    listeners: {
                        complete: Ext.Function.bind(this.onEditorComplete, this)
                    }
                })
            ],
            listeners: {
                selectionchange: Ext.Function.bind(this.onSelectionChange, this),
                render: Ext.Function.bind(this.onRender, this),
                contextmenu: Ext.Function.bind(this.onContextMenu, this)
            }
        });

        // config
        config = Ext.apply(this.initialConfig, {
            items: dataView
        });

        // appy the config
        Ext.apply(this, config);

        // Call parent (required)
        Ext.ux.ThumbsPanel.superclass.initComponent.apply(this, arguments);
    },

    /**
    * @private
    * @param	{Ext.DataView}		dataView	The containing data view
    * @param	{Number}			index		Index of the node that was right clicked
    * @param	{HTMLElement}		node		The node that was right clicked
    * @param	{Ext.EventObject}	evt			Event object
    * @returns	{Void}
    */
    onContextMenu: function (dataView, index, node, evt) {
        var contextMenu;

        evt.preventDefault();
        dataView.select(node);

        contextMenu = this.getGridContextMenu();
        contextMenu.dataView = dataView;
        contextMenu.node = node;
        contextMenu.showAt(evt.getXY());
    }, // eo function onContextMenu

    /**
    * Event handler for when thumbnail has been edited
    * Return false to undo editing
    *
    * @private
    * @param	{Ext.Editor}	editor		The LabelEditor
    * @param	{String}		newValue	New value of the file
    * @param	{String}		oldValue	Old value of the file
    * @returns	{Boolean}					Proceed
    */
    onEditorComplete: function (editor, newValue, oldValue) {
        var extensionChanged;
        // check if the extension of the file was not changed, we dont allow this
        extensionChanged = this.checkExtensionChanged(newValue, oldValue);
        if (extensionChanged) {
            this.alertExtensionChanged(oldValue);
            return false;
        }

        // set the new filename in the record beiing modified
        editor.activeRecord.set(editor.dataIndex, newValue);
        this.renameFile(editor.activeRecord, newValue, oldValue);
        return true;
    }, // eo function onEditorComplete

    /**
    * Event handler for when the selection of the thumbs changes
    * En- or disables buttons in the toolbar depending on the selections
    *
    * @private
    * @param	{Ext.DataView}	dataView	The dataview that contains the thumbs
    * @param	{Array}			selections	The selected nodes
    * @returns	{Void}
    */
    onSelectionChange: function (dataView, selections) {
        if (selections.length > 0) {
            //this.enableGridToolbarButtons();
        } else {
            //this.disableGridToolbarButtons();
        }
    }, // eo function onSelectionChange

    /**
    * Event handler for when the thumbnail panel is rendered
    * Configures dragzone for dataview
    *
    * @private
    * @param	{Ext.DataView} dataView	The DataView from the panel
    * @returns	{Void}
    */
    onRender: function (dataView) {
        // configure new DragZone object
        dataView.dragZone = new Ext.dd.DragZone(dataView.dom || dataView.getEl(), {
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
                        selections: dataView.getSelectedNodes()
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
    } // onRender

});

Ext.ux.ThumbsPanel.prototype.il8n = {
    noFilesText: 'No files to display'
};