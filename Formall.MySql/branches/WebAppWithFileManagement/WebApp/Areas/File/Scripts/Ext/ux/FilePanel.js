
/**
* @class		Ext.ux.FilePanel
* @extends		Ext.Panel
* @namespace	Ext.ux
*
* FilePanel
*
* @author		Rocco Bruyn
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
Ext.define('Ext.ux.FilePanel', {
    extend: 'Ext.Panel',
    layout: 'border',
    border: false,
    /**
    * @cfg {String} dataUrl The URL that is used to process data (required)
    */
    restfulUrl: 'api/v1/File',

    /**
    * @cfg {String} defaultThumbnailImage Default image to use in thumnail view
    */
    defaultThumbnailImage: '../Icons/48/document_blank.png',

    selectionChange: Ext.emptyFn,

    /**
    * Called by Ext when instantiating
    *
    * @private
    * @param {Object} config Configuration object
    */
    initComponent: function () {

        var config;

        var store = Ext.create('Ext.data.Store', {
            storeId: 'fileStore',
            url: 'api/v1/File',
            restful: true,
            proxy: {
                type: 'rest',
                url: 'api/v1/File',
                reader: {
                    type: 'json',
                    root: 'data'
                },
                writer: {
                    type: 'json'
                }
            },
            autoSave: true,
            idProperty: 'Id',
            root: 'data',
            fields: [
                { name: 'Id', type: 'int' },
                { name: 'Name', type: 'string' },
                { name: 'Size', type: 'int' },
                { name: 'Type', type: 'string' },
                { name: 'Date', type: 'date' },
                { name: 'row_class', type: 'string', defaultValue: 'ux-file-icon-unknown-file' },
                { name: 'Thumbnail', type: 'string', defaultValue: this.defaultThumbnailImage }
            ]/*,
            listeners: {
                beforeload: Ext.Function.bind(this.onStoreBeforeLoad, this)
            }*/
        });

        this.tbar = new Ext.Toolbar({
            items: [{
                xtype: 'button',
                text: this.il8n.renameText,
                cmd: 'rename',
                iconCls: 'ux-rename',
                canToggle: true,
                disabled: true,
                handler: Ext.Function.bind(this.onToolbarClick, this)
            }, {
                xtype: 'button',
                text: this.il8n.deleteText,
                cmd: 'delete',
                iconCls: 'ux-delete',
                canToggle: true,
                disabled: true,
                handler: Ext.Function.bind(this.onToolbarClick, this)
            }, '-', {
                xtype: 'button',
                text: this.il8n.uploadText,
                iconCls: 'ux-upload',
                handler: this.fileUploadCallback
            }, /*{
                xtype: 'textfield',
                inputType: 'file',
                fieldLabel: 'Add...',
                autoCreate: { tag: 'input', type: 'text', size: '20', autocomplete: 'off', multiple: 'multiple' },
                iconCls: 'ux-upload',
                text: 'Upload...',
                //labelStyle: 'width: auto',
                //labelWidth: 120,
                name: 'files[]',
                //attributes: 'multiple',
                id: 'uploadfield',
                buttonText: '',
                buttonConfig: {
                    iconCls: 'ux-upload'
                }
            },*/{
            xtype: 'filefield',
            id: 'form-file',
            name: 'photo-path',
            autoCreate: { multiple: 'multiple' },
            allowBlank: false,
            hideLabel: true,
            buttonOnly: true,
            buttonText: 'Add...',
            buttonConfig: {
                iconCls: 'ux-upload'
            },
            anchor: '100%',
            listeners: {
                'change': function (fb, v) {
                    var form = this.up('form').getForm();
                    if (form.isValid()) {
                        form.submit({
                            url: 'service/upload',
                            waitMsg: 'uploading ...',
                            success: function (fp, o) {
                                Ext.Msg.alert('Success', 'Done.');
                            }
                        });
                    }
                }
            }
        }, {
            xtype: 'button',
            text: this.il8n.downloadText,
            cmd: 'download',
            iconCls: 'ux-download',
            canToggle: true,
            disabled: true,
            handler: Ext.Function.bind(this.onToolbarClick, this)
        }, '-', {
            xtype: 'button',
            text: this.il8n.viewsText,
            iconCls: 'ux-view',
            menu: [{
                text: this.il8n.detailsText,
                cmd: 'switchView',
                iconCls: 'ux-details',
                cardIndex: 0,
                viewMode: 'details',
                handler: Ext.Function.bind(this.onToolbarClick, this)
            }, {
                text: this.il8n.thumbsText,
                cmd: 'switchView',
                iconCls: 'ux-thumbs',
                cardIndex: 1,
                viewMode: 'thumbs',
                handler: Ext.Function.bind(this.onToolbarClick, this)
            }]
        }]
    });

    var rowEditor = Ext.create("Ext.ux.grid.plugin.RowEditing", {
        editors: [
            {
                xtype: "textfield",
                name: "Name" //similar to your field name in the model definition
            }
        ],
        listeners: {
            'beforeedit': function(e) {
                var me = this;
                /*if (me.errorSummary && me.isVisible() && !me.autoCancel && me.isDirty()) {
                    me.showToolTip();
                    return false;
                }*/

                /*var allowed = !!me.isEditAllowed;
                if (!me.isEditAllowed)
                    Ext.Msg.confirm('confirm', 'Are you sure you want edit?', function(btn){
                    if (btn !== 'yes')
                        return;
                    me.isEditAllowed = true;
                    me.startEditByPosition({row: e.rowIdx, column: e.colIdx});
                    });
                return allowed;*/
                return true;
            },
            'edit': Ext.Function.bind(this.onGridEditorAfterEdit, this)
        }
    });

    // create a grid that displays files
    this.details = Ext.create('Ext.grid.Panel', {
        cls: 'ux-file-details',
        //title: this.il8n.gridPanelHeaderText,
        border: false,
        stripeRows: true,
        enableDragDrop: true,
        trackMouseOver: true,
        ddGroup: 'fileMoveDD',
        store: store,
        plugins: [rowEditor],
        columns: [
				{
				    header: this.il8n.gridColumnNameHeaderText,
				    id: 'Name',
				    dataIndex: 'Name',
				    sortable: true,
				    editor: new Ext.form.TextField({ vtype: 'filename' })
				}, {
				    header: this.il8n.gridColumnSizeHeaderText,
				    dataIndex: 'Size',
				    sortable: true,
				    renderer: Ext.util.Format.bytesToSi
				}, {
				    header: this.il8n.gridColumnTypeHeaderText,
				    dataIndex: 'Type',
				    sortable: true
				}, {
				    header: this.il8n.gridColumnDateModifiedText,
				    dataIndex: 'Date',
				    sortable: true,
				    renderer: Ext.util.Format.dateRenderer(this.il8n.displayDateFormat)
				}
			],
        selModel: Ext.create('Ext.selection.RowModel', {
            listeners: {
                selectionchange: Ext.Function.bind(this.onGridSelectionChange, this)
            }
        }),
        viewConfig: {
            forceFit: true,
            emptyText: this.il8n.noFilesText,
            getRowClass: function (record, rowIndex, rowParams, store) {
                return 'ux-filepanel-iconrow ' + record.get('row_class');
            }
        },
        listeners: {
            render: Ext.Function.bind(this.onGridRender, this),
            rowcontextmenu: Ext.Function.bind(this.onGridContextMenu, this),
            afteredit: Ext.Function.bind(this.onGridEditorAfterEdit, this)
        }
    });

    // create a panel that will display files as thumbs
    this.thumbs = Ext.create('Ext.ux.ThumbsPanel', {
        cls: 'ux-file-thumbs',
        store: this.details.getStore(),
        onSelectionChange: Ext.Function.bind(this.onThumbsSelectionChange, this),
        checkExtensionChanged: this.checkExtensionChanged,
        alertExtensionChanged: this.alertExtensionChanged
    });

    // config
    config = Ext.apply(this.initialConfig, {
        items: [ this.details, this.thumbs ]
    });

    // appy the config
    Ext.apply(this, config);

    // Call parent (required)
    Ext.ux.FilePanel.superclass.initComponent.apply(this, arguments);

    // flag indicating which 'viewMode' is selected
    // can be 'details' or 'thumbs'
    this.viewMode = 'details';

    // install events
    this.addEvents(
    /**
    * @event BeforeRenameFile
    * Fires before file will be renamed on the server,
    * return false to cancel the event
    *
    * @param {Ext.ux.FilePanel}	this
    * @param {Ext.data.Record}			record	The record representing the file that will be renamed
    * @param {String}					newName	The new file name
    * @param {String}					oldName	The old file name
    * */
			'BeforeRenameFile',

    /**
    * @event BeforeDeleteFile
    * Fires before file will be deleted from the server,
    * return false to cancel the event
    *
    * @param {Ext.ux.FilePanel}	this
    * @param {Ext.data.Record}			record	The record representing the file that will be deleted
    */
			'BeforeDeleteFile',

    /**
    * @event BeforeDownloadFile
    * Fires before file will be downloaded from the server,
    * return false to cancel the event
    *
    * @param {Ext.ux.FilePanel}	this
    * @param {Ext.data.Record}			record	The record representing the file that will be downloaded
    */
			'BeforeDownloadFile',

    /**
    * @event BeforeMoveFile
    * Fires before one or more files will be moved to another folder on the server,
    * return false to cancel the event
    *
    * @param {Ext.ux.FilePanel}	this
    * @param {Array}					files				An array containing Ext.data.Record objects representing the file(s) to move
    * @param {String}					sourceFolder		Path of the source folder
    * @param {String}					destinationFolder	Path of the destination folder
    */
			'BeforeMoveFile',

    /**
    * @event RenameFile
    * Fires when file was successfully renamed
    *
    * @param {Ext.ux.FilePanel}	this
    * @param {Object}					opts	The options that were used for the original request
    * @param {Object}					o		Decoded response body from the server
    */
			'RenameFile',

    /**
    * @event DeleteFile
    * Fires when file(s) was/were successfully deleted
    *
    * @param {Ext.ux.FilePanel}	this
    * @param {Object}					opts	The options that were used for the original request
    * @param {Object}					o		Decoded response body from the server
    */
			'DeleteFile',

    /**
    * @event MoveFile
    * Fires when file(s) was/were successfully moved
    *
    * @param {Ext.ux.FilePanel}	this
    * @param {Object}					opts	The options that were used for the original request
    * @param {Object}					o		Decoded response body from the server
    */
			'MoveFile',

    /**
    * @event RenameFileFailed
    * Fires when renaming file failed
    *
    * @param {Ext.ux.FilePanel}	this
    * @param {Object}					opts	The options that were used for the original request
    * @param {Object}					o		Decoded response body from the server
    */
			'RenameFileFailed',

    /**
    * @event DeleteFileFailed
    * Fires when deleting file(s) failed
    *
    * @param {Ext.ux.FilePanel}	this
    * @param {Object}					opts	The options that were used for the original request
    * @param {Object}					o		Decoded response body from the server
    */
			'DeleteFileFailed',

    /**
    * @event MoveFileFailed
    * Fires when moving file(s) failed
    *
    * @param {Ext.ux.FilePanel}	this
    * @param {Object}					opts	The options that were used for the original request
    * @param {Object}					o		Decoded response body from the server
    */
			'MoveFileFailed'
		);

}, // eo function initComponent

onSetPath: function (path) {
    
    this.disableGridToolbarButtons()

    var store = this.details.getStore();
    var proxy = store.proxy;
    if (proxy.extraParams !== undefined && proxy.extraParams !== null) {
        proxy.extraParams.path = path;
    } else {
        proxy.extraParams = {
            path: path
        };
    }
    store.load();
},

getColumnIndex: function(grid, dataIndex) {
    var gridColumns = grid.headerCt.getGridColumns();
    for (var i = 0; i < gridColumns.length; i++) {
        if (gridColumns[i].dataIndex == dataIndex) {
            return i;
        }
    }
},

/**
* Event handlers for when grid row is right-clicked
* Shows context menu
*
* @private
* @param	{Ext.grid.GridPanel}	grid		Grid panel that was right-clicked
* @param	{Integer}				rowIndex	Index of the selected row
* @param	{Ext.EventObject}		evt			Event object
* @returns	{Void}
*/
onGridContextMenu: function (grid, rowIndex, evt) {
    var contextMenu;

    evt.stopEvent();
    grid.getSelectionModel().selectRow(rowIndex);

    contextMenu = this.getGridContextMenu();
    contextMenu.rowIndex = rowIndex;
    contextMenu.showAt(evt.getXY());
}, // eo function onGridContextMenu

/**
* Event handler for when grid-specific contentmenu item is clicked
* Delegates actions for menu items to other methods
*
* @private
* @param 	{Ext.menu.Menu}		menu		he context menu
* @param	{Ext.menu.Item}		menuItem	The menu item that was clicked
* @param	{Ext.EventObject}	evt			Event object
* @returns	{Void}
*/
onGridContextMenuClick: function (menu, menuItem, evt) {
    var colIndex, labelEditor, record, el, records;
    switch (menuItem.cmd) {
        case 'rename':
            if (this.viewMode === 'details') {
                //colIndex = this.details.getColumnModel().getIndexById('Name');
                //this.details.startEditing(menu.rowIndex, colIndex);
            } else if (this.viewMode === 'thumbs') {
                // get LabelEditor from DataView (first plugin)
                // and the record that is represented by the right-clicked node
                labelEditor = menu.dataView.plugins[0];
                record = menu.dataView.getRecord(menu.node);

                // get the <span> that contains the text
                el = Ext.DomQuery.selectNode(labelEditor.labelSelector, menu.node);

                // invoke editing
                labelEditor.startEdit(el, record.get('Name'));
                labelEditor.activeRecord = record;
            }
            break;

        case 'delete':
            if (this.viewMode === 'details') {
                records = this.details.getSelectionModel().getSelections();
            } else if (this.viewMode === 'thumbs') {
                records = menu.dataView.getSelectedRecords();
            }
            if (records) {
                this.deleteFile(records);
            }
            break;

        case 'download':
            if (this.viewMode === 'details') {
                record = this.details.getSelectionModel().getSelected();
            } else if (this.viewMode === 'thumbs') {
                record = menu.dataView.getRecord(menu.node);
            } else if (this.viewMode === 'slides') {
                record = menu.dataView.getRecords(menu.node);
            }
            if (record) {
                this.downloadFile(record);
            }
            break;

        default:
            break;
    }
}, // eo function onGridContextMenuClick

/**
* Event handler for all button that are clicked in the toolbar
*
* @private
* @param	{Ext.Button}		button	The button that was clicked
* @param	{Ext.EventObject}	evt		The click event
* @returns	{Void}
*/
onToolbarClick: function (button, evt) {
    var rowIndex, colIndex, nameEditor, node, record, el, records;

    if (button.cmd === 'switchView') {
        this.details.getSelectionModel().clearSelections();
        //this.thumbs.get(0).clearSelections();
        //this.slides.get(0).clearSelections();

        this.getLayout().setActiveItem(button.cardIndex);
        this.viewMode = button.viewMode;

    } else if (this.viewMode === 'details') {

        var selModel = this.details.getSelectionModel();
        
        if (button.cmd === 'rename') {
        
            nameEditor = this.details.getPlugin();

            if(nameEditor.editing)
				return false;

            rowIndex = selModel.last;
            colIndex = this.getColumnIndex(this.details, 'Name');
			
            nameEditor.startEdit(rowIndex, colIndex);
        } else {
            switch (button.cmd) {
                case 'delete':
                    records = selModel().getSelections();
                    if (records) {
                        this.deleteFile(records);
                    }
                    break;
                case 'download':
                    record = selModel.getSelected();
                    if (record) {
                        this.downloadFile(record);
                    }
                    break;
                default:
                    break;
            }
        }

    } else {

        var dataView;

        if (this.viewMode === 'thumbs') {
            // get the DataView from the thumbs panel (first item)
            dataView = this.thumbs.get(0);
        } else if (this.viewMode === 'slides') {
            // get the DataView from the slides panel (first item)
            dataView = this.slides.get(0);
        }
        
        // get the first selected node which is an HTMLElement
        node = dataView.getSelectedNodes()[0];
        
        // make sure only one node is selected
        dataView.select(node);

        if (button.cmd === 'rename') {

            // then get the LabelEditor from the DataView (first plugin)
            nameEditor = dataView.plugins[0];

            // get the <span> that contains the text
            el = Ext.DomQuery.selectNode(nameEditor.labelSelector, node);

            // invoke editing
            nameEditor.startEdit(el, record.get('Name'));
            nameEditor.activeRecord = record;
        } else {
            // get the record that it represents
            record = dataView.getRecord(node);

            if (record) {
                switch (button.cmd) {
                    case 'delete':
                        this.deleteFile(record);
                        break;
                    case 'download':
                        this.downloadFile(record);
                        break;
                    default:
                        break;
                }
            }
        }
    }

}, // eo function onToolbarClick

/**
* Event handler for when filename is changed
* Checks if new filename doesn't already exist
*
* @private
* @param	{Ext.EventObject}	event	Edit event object
* @returns	{Boolean}					Success
*/
onGridEditorAfterEdit: function (evt/*editor*/, context) {
    //delete context.record._blank;
    var extensionChanged;
    var field = evt.column.dataIndex;
    var originalValue = evt.originalValues[field];
    var newValue = evt.newValues[field];
    // check if the extension was changed, we don't allow this

    extensionChanged = this.checkExtensionChanged(newValue, originalValue);
    if (extensionChanged) {
        this.alertExtensionChanged(evt.originalValue);
        evt.record.reject();
        return false;
    }

    this.renameFile(evt.record, newValue, originalValue);
    return true;
}, // eo function onGridEditorAfterEdit


/**
* Event handler for when selection in the grid changes
* En- or disables buttons in the toolbar depending on selection in the grid
*
* @private
* @param	{Ext.create('Ext.selection.RowModel', {})} sm The selection model
* @returns	{Void}
*/
onGridSelectionChange: function (sm) {
    if (sm.hasSelection()) {
        this.enableGridToolbarButtons();
    } else {
        this.disableGridToolbarButtons();
    }
}, // eo function onGridSelectionChange

    /**
    * Event handler for when the selection of the thumbs changes
    * En- or disables buttons in the toolbar depending on the selections
    *
    * @private
    * @param	{Ext.DataView}	dataView	The dataview that contains the thumbs
    * @param	{Array}			selections	The selected nodes
    * @returns	{Void}
    */
    onThumbsSelectionChange: function (dataView, selections) {
        if (selections.length > 0) {
            this.enableGridToolbarButtons();
        } else {
            this.disableGridToolbarButtons();
        }
    }, // eo function onThumbsSelectionChange

    /**
    * Event handler for when the grid is about to load new data
    * Appends the folder path of the selected node to the request
    *
    * @private
    * @param	{Ext.data.JsonStore}	store	The store object
    * @param	{Object}				opts	Loading options
    * @returns	{Void}
    */
    onGridStoreBeforeLoad: function (store, opts) {
        /*
        var node;
        store.extraParams.action = 'GetFiles';
				
        //store.setBaseParam('path', node.getPath('text'));
        console.log('gridBeforeload');
        console.log(store.extraParams);
        */
    }, // eo function onGridStoreBeforeLoad

/**
* Event handler when the grid is rendered
* Creates a new tooltip that shows on the grid rows
*
* @private
* @param	{Ext.grid.GridPanel} grid The grid panel
* @returns	{Void}
*/
onGridRender: function (grid) {
    this.tip = Ext.create('Ext.ToolTip', {
        view: this.details.getView(),
        target: this.details.getView().mainBody,
        delegate: '.x-grid3-row',
        renderTo: Ext.getBody(),
        listeners: {
            beforeshow: function (tip) {
                var text;
                text = Ext.DomQuery.selectValue('td:first div', tip.triggerElement);
                tip.body.update(text);
            }
        }
    });
}, // eo function onGridRender

/**
* Gets and lazy creates context menu for file grid
*
* @private
* @returns {Ext.menu.Menu} Context menu
*/
getGridContextMenu: function () {

    if (!this.detailsContextMenu) {
        this.detailsContextMenu = new Ext.menu.Menu({
            items: [{
                text: this.il8n.renameText,
                cmd: 'rename',
                iconCls: 'ux-file-icon-RenameFile'
            }, {
                text: this.il8n.deleteText,
                cmd: 'delete',
                iconCls: 'ux-file-icon-DeleteFile'
            }, {
                text: this.il8n.downloadText,
                cmd: 'download',
                iconCls: 'ux-file-icon-DownloadFile'
            }],
            listeners: {
                click: {
                    fn: this.onGridContextMenuClick,
                    scope: this
                }
            }
        });
    }

    return this.detailsContextMenu;
}, // eo function getGridContextMenu

/**
* Checks if the extension of a renamed file was not changed
*
* @private
* @param	{String}	newName		New filename
* @param	{String}	oldName		Old filename
* @returns	{Boolean}				If extension was changed
*/
checkExtensionChanged: function (newName, oldName) {
    var matchNew, matchOld;
    matchNew = Ext.form.VTypes.filenameVal.exec(newName);
    matchOld = Ext.form.VTypes.filenameVal.exec(oldName);
    return (matchNew[1] !== matchOld[1]);
}, // eo function checkExtensionChanged

/**
* Alert user that filename cannot be changed because
* the extension may not change
*
* @private
* @param	{String}	fileName	The original filename
* @param	{Function}	callback	Optional callback function to execute after the alert
* @returns	{Void}
*/
alertExtensionChanged: function (fileName, callback) {
    Ext.Msg.show({
        title: this.il8n.extensionChangeTitleText,
        msg: String.format(this.il8n.extensionChangeMsgText, fileName),
        buttons: Ext.Msg.OK,
        icon: Ext.Msg.ERROR,
        closable: false,
        fn: callback || Ext.EmptyFn
    });
}, // eo function alertExtensionChanged

/**
* Disables buttons in the toolbar that are marked 'toggleable'
*
* @private
* @returns {Void}
*/
disableGridToolbarButtons: function () {
    var buttons;
    buttons = this.getDockedItems('toolbar[dock="top"]')[0].query('button[canToggle=true]');
    Ext.each(buttons, function (button) {
        button.disable();
    });
}, // eo function disableGridToolbarButtons

/**
* Enables buttons in the toolbar that are marked 'toggleable'
*
* @private
* @returns {Void}
*/
enableGridToolbarButtons: function () {
    var buttons;
    buttons = this.getDockedItems('toolbar[dock="top"]')[0].query('button[canToggle=true]');
    Ext.each(buttons, function (button) {
        button.enable();
    });
}, // eo function enableGridToolbarButtons

showErrorMessage: function () {
    Ext.Msg.show({
        title: this.il8n.actionRequestFailureTitleText,
        msg: this.il8n.actionRequestFailureMsgText,
        buttons: Ext.Msg.OK,
        icon: Ext.Msg.ERROR,
        closable: false
    });
},

processResponse: function (response) {
    var o = {}, store, record;

    // decode response in try..catch, response might be mangled/incorrect
    // show error message in case of failure
    try {
        o = Ext.decode(response.responseText);
    } catch (e) {

        o = undefined;

        Ext.Msg.show({
            title: this.il8n.actionResponseFailureTitleText,
            msg: this.il8n.actionResponseFailureMsgText,
            buttons: Ext.Msg.OK,
            icon: Ext.Msg.ERROR,
            closable: false
        });
    }

    return o;
},

/**
* Callback that handles all actions performed on the server (rename, move etc.)
* Called when Ajax request finishes, regardless if this was a success or not
*
* @private
* @param	{Object}	opts		The options that were used for the original request
* @param	{Boolean}	success		If the request succeded
* @param	{Object}	response	The XMLHttpRequest object containing the response data
* @returns	{Void}
*/

actionCallback: function (opts, success, response) {
    var o = {}, store, record;

    // check if request was successful
    if (true !== success) {
        Ext.Msg.show({
            title: this.il8n.actionRequestFailureTitleText,
            msg: this.il8n.actionRequestFailureMsgText,
            buttons: Ext.Msg.OK,
            icon: Ext.Msg.ERROR,
            closable: false
        });
        return;
    }

    // decode response in try..catch, response might be mangled/incorrect
    // show error message in case of failure
    try {
        o = Ext.decode(response.responseText);
    } catch (e) {
        Ext.Msg.show({
            title: this.il8n.actionResponseFailureTitleText,
            msg: this.il8n.actionResponseFailureMsgText,
            buttons: Ext.Msg.OK,
            icon: Ext.Msg.ERROR,
            closable: false
        });
    }

    var action = (opts.params ? opts.params.Action : opts.jsonData ? opts.jsonData.Action : null) || 'None';

    // check if server reports all went well
    // handle success/failure accordingly
    if (true === o.success) {
        switch (action) {
            case 'DeleteFile':
                // fire DeleteFile event
                if (true !== this.eventsSuspended) {
                    this.fireEvent('DeleteFile', this, opts, o);
                }

                // delete record(s) from the grid
                store = this.details.getStore();
                Ext.each(o.data.successful, function (item, index, allItems) {
                    record = store.getById(item.Id);
                    store.remove(record);
                });
                break;
            case 'MoveFile':
                // fire MoveFile event
                if (true !== this.eventsSuspended) {
                    this.fireEvent('MoveFile', this, opts, o);
                }

                // delete record(s) from the grid
                store = this.details.getStore();
                Ext.each(o.data.successful, function (item, index, allItems) {
                    record = store.getById(item.Id);
                    store.remove(record);
                });
                break;
            case 'RenameFile':
                // fire RenameFile event
                if (true !== this.eventsSuspended) {
                    this.fireEvent('RenameFile', this, opts, o);
                }
                // commit the change in the record
                opts.record.commit();
                break;
            case 'UploadFile':
                break;
            default:
                break;
        }
    } else {
        switch (action) {
            case 'DeleteFile':
                // fire DeleteFileFailed event
                if (true !== this.eventsSuspended) {
                    this.fireEvent('DeleteFileFailed', this, opts, o);
                }
                // delete successfully moved record(s) from the grid
                store = this.details.getStore();
                Ext.each(o.data.successful, function (item, index, allItems) {
                    record = store.getById(item.id);
                    store.remove(record);
                });
                break;
            case 'MoveFile':
                // fire MoveFileFailed event
                if (true !== this.eventsSuspended) {
                    this.fireEvent('MoveFileFailed', this, opts, o);
                }
                // delete successfully moved record(s) from the grid
                store = this.details.getStore();
                Ext.each(o.data.successful, function (item, index, allItems) {
                    record = store.getById(item.id);
                    store.remove(record);
                });
                // prompt for overwrite
                if (o.data.existing.length > 0) {
                    Ext.Msg.show({
                        title: this.il8n.confirmOverwriteTitleText,
                        msg: this.il8n.confirmOverwriteMsgText,
                        buttons: Ext.Msg.YESNO,
                        icon: Ext.Msg.QUESTION,
                        closable: false,
                        scope: this,
                        fn: function (buttonId, text, cfg) {
                            var files, store;
                            if (buttonId === 'yes') {
                                // create array with remaining files
                                files = [];
                                store = this.details.getStore();
                                Ext.each(o.data.existing, function (item, index, allItems) {
                                    files.push(store.getById(item.id));
                                });

                                // call again, but with overwrite option
                                this.moveFile(files, opts.params.sourcePath, opts.params.destinationPath, true);
                            }
                        }
                    });
                }
                break;
            case 'RenameFile':
                // fire RenameFileFailed event
                if (true !== this.eventsSuspended) {
                    this.fireEvent('RenameFileFailed', this, opts, o);
                }
                // reject the change in the record
                opts.record.reject();
                break;
            case 'UploadFile':
                break;
            default:
                break;
        }
    }
}, // eo function actionCallback

/**
* Deletes file from the server
*
* @private
* @param	{Array}	files Array of Ext.data.Record objects representing the files that need to be deleted
* @returns	{Void}
*/
deleteFile: function (files) {
    var jsonData, dialogTitle, dialogMsg;
    // fire BeforeDeleteFile event
    if (true !== this.eventsSuspended &&
		   false === this.fireEvent('BeforeDeleteFile', this, files)) {
        return;
    }

    // set request data
    jsonData = {
        Action: "DeleteFile",
        Path: this.details.getStore().proxy.extraParams.path + '/',
        Files: []
    };

    // loop over files array and add request data like:
    Ext.each(files, function (item, index, allItems) {
        jsonData.Files.push({ Id: item.id || item.get('Id'), Name: item.get('Name') });
    });

    // prepare confirmation texts depending on amount of files
    dialogTitle = this.il8n.confirmDeleteSingleFileTitleText;
    dialogMsg = String.format(this.il8n.confirmDeleteSingleFileMsgText, files[0].get('Name'));
    if (files.length > 1) {
        dialogTitle = this.il8n.confirmDeleteMultipleFileTitleText;
        dialogMsg = String.format(this.il8n.confirmDeleteMultipleFileMsgText, files.length);
    }

    // confirm removal
    Ext.Msg.show({
        title: dialogTitle,
        msg: dialogMsg,
        buttons: Ext.Msg.YESNO,
        icon: Ext.Msg.QUESTION,
        closable: false,
        scope: this,
        fn: function (buttonId) {
            if (buttonId === 'yes') {
                // send request to server
                Ext.Ajax.request({
                    method: 'DELETE',
                    url: 'api/v1/File',
                    jsonData: jsonData,
                    restful: true,
                    callback: this.actionCallback,
                    scope: this
                });
            }
        }
    });
}, // eo function deleteFile

/**
* Download a file from the server
* Shamelessly stolen from Saki's FileTreePanel (Saki FTW! :p)
* But it does exactly what i need it to do, and it does it very well..
* @see http://filetree.extjs.eu/
*
* @private
* @param	{Ext.data.Record} record Record representing the file that needs to be downloaded
* @returns	{Void}
*/
downloadFile: function (record) {
    var id, frame, form, hidden, callback;
    // fire BeforeDownloadFile event
    if (true !== this.eventsSuspended &&
		   false === this.fireEvent('BeforeDownloadFile', this, record)) {
        return;
    }

    // generate a new unique id
    id = Ext.id();

    // create a new iframe element
    frame = document.createElement('iframe');
    frame.id = id;
    frame.name = id;
    frame.className = 'x-hidden';

    // use blank src for Internet Explorer
    if (Ext.isIE) {
        frame.src = Ext.SSL_SECURE_URL;
    }

    // append the frame to the document
    document.body.appendChild(frame);

    // also set the name for Internet Explorer
    if (Ext.isIE) {
        document.frames[id].name = id;
    }

    //  create a new form element
    form = Ext.DomHelper.append(document.body, {
        tag: 'form',
        method: 'post',
        action: this.dataUrl,
        target: id
    });

    // create hidden input element with the 'action'
    hidden = document.createElement('input');
    hidden.type = 'hidden';
    hidden.name = 'action';
    hidden.value = 'download-file';
    form.appendChild(hidden);

    // create another hidden element that holds the path of the file to download
    hidden = document.createElement('input');
    hidden.type = 'hidden';
    hidden.name = 'path';
    hidden.value = this.getSelectedFilePath();
    form.appendChild(hidden);

    // create a callback function that does some cleaning afterwards
    callback = function () {
        Ext.EventManager.removeListener(frame, 'load', callback, this);
        setTimeout(function () {
            document.body.removeChild(form);
        }, 100);
        setTimeout(function () {
            document.body.removeChild(frame);
        }, 110);
    };

    // attach callback and submit the form
    Ext.EventManager.on(frame, 'load', callback, this);
    form.submit();
},

/**
* Move a file on the server to another folder
*
* @private
* @param	{Array}		files				Array of Ext.data.Record objects representing the files to move
* @param	{String}	sourceFolder		Source folder
* @param	{String}	destinationFolder	Destination folder
* @param	{Boolean}	overwrite			If files should be overwritten in destination, defaults to false
* @returns	{Void}
*/
moveFile: function (files, sourceFolder, destinationFolder, overwrite) {
    var params;
    // fire BeforeMoveFile event
    if (true !== this.eventsSuspended &&
		   false === this.fireEvent('BeforeMoveFile', this, files, sourceFolder, destinationFolder)) {
        return;
    }

    // set request data
    var jsonData = {
        Action: 'MoveFile',
        Path: destinationFolder,
        Files: []
        /*overwrite: (true === overwrite) ? true : false*/
    };

    // loop over files array and add request parameters like:
    // files[rec-id] : filename.ext
    Ext.each(files, function (item, index, allItems) {
        jsonData.Files.push({ Id: item.id || item.get('Id'), Name: item.get('Name') });
    });

    // send request to server
    Ext.Ajax.request({
        url: 'api/v1/File',
        method: 'PUT',
        jsonData: jsonData,
        restful: true,
        sourcePath: sourceFolder,
        destinationPath: destinationFolder,
        callback: this.actionCallback,
        scope: this
    });
}, // eo function moveFile

/**
* Renames file on the server
*
* @private
* @param	{Ext.data.Record}	record	Record representing the file that is beiing renamed
* @param	{String}			newName	New filename
* @param	{String}			oldName	Old filname
* @returns	{Void}
*/
renameFile: function (record, newName, oldName) {
    // fire BeforeRenameFile event
    if (true !== this.eventsSuspended &&
		   false === this.fireEvent('BeforeRenameFile', this, record, newName, oldName)) {
        return;
    }

    // set request data
    var jsonData = {
        Action: "RenameFile",
        Path: this.details.getStore().proxy.extraParams.path + '/',
        Files: [{ Id: record.data.Id, Name: newName}]
    };

    // send request to server
    Ext.Ajax.request({
        url: 'api/v1/File',
        method: 'PUT',
        jsonData: jsonData,
        restful: true,
        record: record,
        newName: newName,
        oldName: oldName,
        callback: this.actionCallback,
        scope: this
    });

}, // eo function renameFile

/**
* Uploads file to the server
*
* @private
* @param	{Array}	files Array of Ext.data.Record objects representing the files that need to be deleted
* @returns	{Void}
*/
uploadFile: function (files) {
},

/**
* Refreshes the grid with data from the server
*
* @returns	{Void}
*/
refreshGrid: function () {
    this.details.getStore().load();
}, // eo function refreshGrid

/**
* Gets the name of the current selected file
*
* @returns {String} The name of the current selected file
*/
getSelectedFileName: function () {
    var record, dataView;

    // get the selected node from the grid or thumbs, depending on viewMode
    if (this.viewMode === 'details') {
        record = this.details.getSelectionModel().getSelected();
    } else if (this.viewMode === 'thumbs') {
        dataView = this.thumbs.get(0);
        record = dataView.getSelectedRecords()[0];
    }

    // check if they are both present
    if (record === undefined) {
        return null;
    }

    return record.get('Name');
}, // eo function getSelectedFileName

/**
* Gets the full path of the current selected file
*
* @returns {String} The full path of the current selected file
*/
getSelectedFilePath: function () {
    return getPath() + getSelectedFileName();
}, // eo function getSelectedFilePath

/**
* Gets the current path
*
* @returns {String} The current path
*/
getPath: function () {
    return this.path;
}, // eo function getPath

});                                   // eo extend

// register xtype
//Ext.reg('ux-FilePanel', Ext.ux.FilePanel);

/**
* Strings for internationalization
*/
Ext.ux.FilePanel.prototype.il8n = {
    displayDateFormat: 'd/m/Y H:i',
    newText: 'New',
    renameText: 'Rename',
    deleteText: 'Delete',
    uploadText: 'Upload',
    downloadText: 'Download',
    viewsText: 'Views',
    detailsText: 'Details',
    thumbsText: 'thumbs',
    newFolderText: 'New-Folder',
    noFilesText: 'No files to display',
    treePanelHeaderText: 'Folders',
    gridPanelHeaderText: 'Files',
    gridColumnNameHeaderText: 'Name',
    gridColumnSizeHeaderText: 'Size',
    gridColumnTypeHeaderText: 'Type',
    gridColumnDateModifiedText: 'Date Modified',
    extensionChangeTitleText: 'Error changing extension',
    extensionChangeMsgText: "Cannot rename '{0}'. You cannot change the file extension.",
    confirmDeleteFolderTitleText: 'Confirm folder delete',
    confirmDeleteFolderMsgText: "Are you sure you want to remove the folder '{0}' and all of it's contents?",
    confirmDeleteSingleFileTitleText: 'Confirm file delete',
    confirmDeleteSingleFileMsgText: "Are you sure you want to delete '{0}'?",
    confirmDeleteMultipleFileTitleText: 'Confirm multiple file delete',
    confirmDeleteMultipleFileMsgText: "Are you sure you want to delete these {0} files?",
    confirmOverwriteTitleText: 'Confirm file replace',
    confirmOverwriteMsgText: 'One or more files with the same name already exist in the destination folder. Do you wish to overwrite these?',
    actionRequestFailureTitleText: 'Oh dear..',
    actionRequestFailureMsgText: "It seems like your colleague spilled coffee on your keyboard. We can't send your request until you hang it out to dry",
    actionResponseFailureTitleText: 'PANIC!!',
    actionResponseFailureMsgText: 'Pink elephants are stampeding through the server! Run for the hills!'
};

/**
* Additional Format function(s) to use
*/
Ext.apply(Ext.util.Format, {
    /**
    * Format filesize to human readable format
    * Also deals with filesizes in units larger then MegaBytes
    *
    * @param	{Integer}	size	Filesize in bytes
    * @returns	{String}			Formatted filesize
    */
    bytesToSi: function (size) {
        if (typeof size === 'number' && size > 0) {
            var s, e, r;
            s = ['b', 'Kb', 'Mb', 'Gb', 'Tb', 'Pb', 'Eb', 'Zb', 'Yb'];
            e = Math.floor(Math.log(size) / Math.log(1024));
            r = size / Math.pow(1024, e);
            if (Math.round(r.toFixed(2)) !== r.toFixed(2)) {
                r = r.toFixed(2);
            }
            return r + ' ' + s[e];
        } else {
            return '0 b';
        }
    }
}); // eo apply

/**
* Additional VType(s)to use
*/
Ext.apply(Ext.form.VTypes, {
    /**
    * Validation type for filenames
    * allows only alphanumeric, underscore, hypen and dot
    * Checks for extension between 2 and 4 karakters
    */
    filenameVal: /[a-z0-9_\-\.]+\.([a-z0-9]{2,4})$/i,
    filenameMask: /[a-z0-9_\-\.]/i,
    filenameText: 'Filename is invalid or contains illegal characters',
    filename: function (val, field) {
        return Ext.form.VTypes.filenameVal.test(val);
    }

}); // eo apply

// eof
