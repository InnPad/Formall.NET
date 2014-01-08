
/**
* @class		Ext.ux.FolderTree
* @extends		Ext.Panel
* @namespace	Ext.ux
*
* FolderTree
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
Ext.ux.FolderTree = Ext.extend(Ext.tree.TreePanel, {
    /**
    * @cfg {String} dataUrl The URL that is used to process data (required)
    */
    restfulUrl: 'api/v1/Folder',

    cls: 'ux-folder-tree',

    border: false,

    useArrows: true,

    enableDrop: true,

    selectionchange: {
        fn: Ext.emptyFn,
        scope: null
    },

    /**
    * Called by Ext when instantiating
    *
    * @private
    * @param {Object} config Configuration object
    */
    initComponent: function () {

        this.title = this.il8n.treePanelHeaderText;

        this.dropConfig = {
            ddGroup: 'fileMoveDD',
            appendOnly: true,
            onNodeDrop: this.onTreeNodeDrop.createDelegate(this)
        };
        this.loader = new Ext.tree.TreeLoader({
            url: this.restfulUrl,
            requestMethod: 'GET',
            listeners: {
                beforeload: {
                    fn: this.onTreeLoaderBeforeLoad,
                    scope: this
                }
            }
        });
        this.root = new Ext.tree.AsyncTreeNode({
            text: 'Root',
            expanded: true
        });
        this.selModel = new Ext.tree.DefaultSelectionModel({
            listeners: {
                selectionchange: {
                    fn: this.onTreeSelectionChange,
                    scope: this
                }
            }
        });
        this.tools = [{
            id: 'plus',
            qtip: this.il8n.newText,
            handler: this.onPlusToolClick,
            scope: this
        }, {
            id: 'minus',
            qtip: this.il8n.deleteText,
            handler: this.onMinusToolClick,
            scope: this,
            hidden: true
        }, {
            id: 'gear',
            qtip: this.il8n.renameText,
            handler: this.onGearToolClick,
            scope: this,
            hidden: true
        }];
        this.listeners = {
            contextmenu: {
                fn: this.onTreeContextMenu,
                scope: this
            },
            afterrender: {
                fn: this.onTreeAfterRender,
                scope: this,
                single: true
            }
        };

        // create an editor for the tree for modifying folder names
        this.treeEditor = new Ext.tree.TreeEditor(this, {
            allowBlank: false,
            grow: true,
            growMin: 90,
            growMax: 240
        }, {
            completeOnEnter: true,
            cancelOnEsc: true,
            ignoreNoChange: true,
            selectOnFocus: true,
            listeners: {
                beforestartedit: {
                    fn: this.onTreeEditorBeforeStartEdit,
                    scope: this
                },
                beforecomplete: {
                    fn: this.onTreeEditorBeforeComplete,
                    scope: this
                }
            }
        });

        // Call parent (required)
        Ext.ux.FolderTree.superclass.initComponent.apply(this, arguments);

        // install events
        this.addEvents(
        /**
        * @event BeforeCreateFolder
        * Fires before a new folder is created on the server,
        * return false to cancel the event
        *
        * @param {Ext.ux.FolderTree}	this
        * @param {Ext.tree.TreeNode}		node The node representing the folder to be created
        */
			'BeforeCreateFolder',

        /**
        * @event BeforeRenameFolder
        * Fires before folder will be renamed on the server,
        * return false to cancel the event
        *
        * @param {Ext.ux.FolderTree}	this
        * @param {Ext.tree.TreeNode}		node		The node representing the folder that will be renamed
        * @param {String}					newName		The new folder name
        * @param {String}					oldName		The old folder name
        */
			'BeforeRenameFolder',

        /**
        * @event BeforeDeleteFolder
        * Fires before folder will be deleted on the server,
        * return false to cancel the event
        *
        * @param {Ext.ux.FolderTree}	this
        * @param {Ext.tree.TreeNode}		node The node representing the folder that will be deleted
        */
			'BeforeDeleteFolder',

        /**
        * @event CreateFolder
        * Fires when folder was successfully created
        *
        * @param {Ext.ux.FolderTree}	this
        * @param {Object}					opts	The options that were used for the original request
        * @param {Object}					o		Decoded response body from the server
        */
			'CreateFolder',

        /**
        * @event RenameFolder
        * Fires when folder was successfully renamed
        *
        * @param {Ext.ux.FolderTree}	this
        * @param {Object}					opts	The options that were used for the original request
        * @param {Object}					o		Decoded response body from the server
        */
			'RenameFolder',

        /**
        * @event DeleteFolder
        * Fires when folder was successfully deleted
        *
        * @param {Ext.ux.FolderTree}	this
        * @param {Object}					opts	The options that were used for the original request
        * @param {Object}					o		Decoded response body from the server
        */
			'DeleteFolder',

        /**
        * @event CreateFolderFailed
        * Fires when creation of folder failed
        *
        * @param {Ext.ux.FolderTree}	this
        * @param {Object}					opts	The options that were used for the original request
        * @param {Object}					o		Decoded response body from the server
        */
			'CreateFolderFailed',

        /**
        * @event RenameFolderFailed
        * Fires when renaming folder failed
        *
        * @param {Ext.ux.FolderTree}	this
        * @param {Object}					opts	The options that were used for the original request
        * @param {Object}					o		Decoded response body from the server
        */
			'RenameFolderFailed',

        /**
        * @event DeleteFolderFailed
        * Fires when deleting folder failed
        *
        * @param {Ext.ux.FolderTree}	this
        * @param {Object}					opts	The options that were used for the original request
        * @param {Object}					o		Decoded response body from the server
        */
			'DeleteFolderFailed'

		);

    }, // eo function initComponent

    /**
    * Event handler for when 'plus' tool in the tree header is clicked
    * Invokes creation of new folder
    *
    * @private
    * @param	{Ext.EventObject}	evt		The click event
    * @param	{Ext.Element}		toolEl	The tool element
    * @param	{Ext.Panel}			panel	The host panel
    * @param	{Ext.Panel}			tc		The tool config object
    * @returns	{Void}
    */
    onPlusToolClick: function (evt, toolEl, panel, tc) {
        var node;
        node = this.getSelectionModel().getSelectedNode();
        this.invokeCreateFolder(node);
    }, // eo function onPlusToolClick

    /**
    * Event handler for when 'minus' tool in the tree header is clicked
    * Invokes deletion of selected folder
    *
    * @private
    * @param	{Ext.EventObject}	evt		The click event
    * @param	{Ext.Element}		toolEl	The tool element
    * @param	{Ext.Panel}			panel	The host panel
    * @param	{Ext.Panel}			tc		The tool config object
    * @returns	{Void}
    */
    onMinusToolClick: function (evt, toolEl, panel, tc) {
        var node;
        node = this.getSelectionModel().getSelectedNode();
        this.deleteFolder(node);
    }, // oe onMinusToolClick

    /**
    * Event handler for when 'gear' tool in the tree header is clicked
    * Invokes renaming of selected folder
    *
    * @private
    * @param	{Ext.EventObject}	evt		The click event
    * @param	{Ext.Element}		toolEl	The tool element
    * @param	{Ext.Panel}			panel	The host panel
    * @param	{Ext.Panel}			tc		The tool config object
    * @returns	{Void}
    */
    onGearToolClick: function (evt, toolEl, panel, tc) {
        var node;
        node = this.getSelectionModel().getSelectedNode();
        this.treeEditor.triggerEdit(node);
    }, // eo function onGearToolClick

    /**
    * Event handler for when tree node is right-clicked
    * Shows context menu
    *
    * @private
    * @param	{Ext.tree.TreeNode}	node	Tree node that was right-clicked
    * @param	{Ext.EventObject}	evt		Event object
    * @returns	{Void}
    */
    onTreeContextMenu: function (node, evt) {
        var contextMenu;

        evt.stopEvent();
        node.select();

        contextMenu = this.getTreeContextMenu();
        contextMenu.find('text', this.il8n.renameText)[0].setDisabled(node.isRoot);
        contextMenu.find('text', this.il8n.deleteText)[0].setDisabled(node.isRoot);
        contextMenu.showAt(evt.getXY());
    }, // eo function onTreeContextMenu

    /**
    * Event handler for when tree-specific contextmenu item is clicked
    * Delegates actions for menu items to other methods
    *
    * @private
    * @param 	{Ext.menu.Menu}		menu		he context menu
    * @param	{Ext.menu.Item}		menuItem	The menu item that was clicked
    * @param	{Ext.EventObject}	evt			Event object
    * @returns	{Void}
    */
    onTreeContextMenuClick: function (menu, menuItem, evt) {
        var node;
        node = this.getSelectionModel().getSelectedNode();
        switch (menuItem.cmd) {
            case 'new':
                this.invokeCreateFolder(node);
                break;
            case 'rename':
                this.treeEditor.triggerEdit(node);
                break;

            case 'delete':
                this.deleteFolder(node);
                break;
        }

    }, // eo function onTreeContextMenuClick

    /**
    * Event handler for when editit of treenode is initiated, but before the value changes
    * Checks if node is not the root node, since it's not allowed to change that
    *
    * @param	{Ext.tree.TreeEditor}	editor	The tree editor
    * @param	{Ext.Element}			boundEl	Underlying element
    * @param	{Mixed}					value	Field value
    * @returns	{Boolean}						Proceed?
    */
    onTreeEditorBeforeStartEdit: function (editor, boundEl, value) {
        return (editor.editNode.isRoot) ? false : true;
    }, // eo function onTreeEditorBeforeStartEdit

    /**
    * Event handler for when node has been edited but change is not yet been
    * reflected in the underlying field
    * Return false to undo editing
    *
    * @private
    * @param	{Ext.tree.TreeEditor}	editor		The tree editor where the node was edited
    * @param	{String}				newValue	New value of the node
    * @param	{String}				oldValue	Old value of the node
    * @returns	{Boolean}							Proceed?
    */
    onTreeEditorBeforeComplete: function (editor, newValue, oldValue) {
        // set the new folder name in the node beiing renamed
        editor.editNode.setText(newValue);
        // check if the node beiing edited is a new or existing folder
        if (editor.editNode.attributes.isNew) {
            this.createFolder(editor.editNode);
            this.treeEditor.ignoreNoChange = true;
        } else {
            this.renameFolder(editor.editNode, newValue, oldValue);
        }

        return true;
    }, // eo function onTreeEditorBeforeComplete

    /**
    * Event handler for when the selection changes in the tree
    * Appends the folder path of the selected node to the request
    * Causes files to be loaded in the grid for selected node
    * Toggles displaying of 'minus' and 'gear' tools depending on selection
    *
    * @private
    * @param	{Ext.tree.DefaultSelectionModel}	sm		Selection model
    * @param	{Ext.tree.TreeNode}					node	The selected tree node
    * @returns	{Void}
    */
    onTreeSelectionChange: function (sm, node) {
        this.getTool('minus').setDisplayed((!node.isRoot));
        this.getTool('gear').setDisplayed((!node.isRoot));

        if (this.selectionChange) {
            this.selectionChange(sm.getSelectedNode().getPath('text'));
        }
    }, // eo function onTreeSelectionChange

    /**
    * Event handler for when the tree is about to load new data
    * Appends the folder path of the selected node to the request
    *
    * @private
    * @param	{Ext.tree.TreeLoader}	TreeLoader	The Treeloader
    * @param	{Ext.tree.TreeNode}		node		The selected node
    * @param	{Object}				callback	Callback function specified in the 'load' call from the treeloader
    * @returns	{Void}
    */
    onTreeLoaderBeforeLoad: function (treeLoader, node, callback) {
        treeLoader.baseParams.action = 'LoadFolder';
        treeLoader.baseParams.path = node.getPath('text');
    }, // eo function onTreeLoaderBeforeLoad

    /**
    * Event handler for when the tree is renderer
    * Selects the root node, causing the files in the root to be loaded
    *
    * @private
    * @param	{Ext.tree.TreePanel} tree The tree panel
    * @returns	{Void}
    */
    onTreeAfterRender: function (tree) {
        tree.getSelectionModel().select(tree.root);
    }, // eo function onTreeRender

    /**
    * Event handler for when a file from the grid is dropped on a folder
    * in the tree
    *
    * @private
    * @param	{Object}			nodeData	Custom data associated with the drop node
    * @param	{Ext.dd.DragSource}	source		The source that was dragged over this dragzone
    * @param	{Ext.EventObject}	evt			The Event object
    * @param	{Object}			data		Object containing arbitrairy data supplied by drag source
    * @returns	{Boolean}
    */
    onTreeNodeDrop: function (nodeData, source, evt, data) {
        var from, to;
        // get the source- and destination folder
        from = this.getSelectionModel().getSelectedNode().getPath('text');
        to = nodeData.node.getPath('text');

        // move the file
        this.moveFile(data.selections, from, to);
        return true;
    }, // eo function onTreeNodeDrop

    /**
    * Gets and lazy creates context menu for folder tree
    *
    * @private
    * @returns {Ext.menu.Menu} Context menu
    */
    getTreeContextMenu: function () {

        if (!this.treeContextMenu) {
            this.treeContextMenu = new Ext.menu.Menu({
                items: [{
                    text: this.il8n.newText,
                    cmd: 'new',
                    iconCls: 'ux-newfolder'
                }, {
                    text: this.il8n.renameText,
                    cmd: 'rename',
                    iconCls: 'ux-RenameFolder'
                }, {
                    text: this.il8n.deleteText,
                    cmd: 'delete',
                    iconCls: 'ux-DeleteFolder'
                }],
                listeners: {
                    click: {
                        fn: this.onTreeContextMenuClick,
                        scope: this
                    }
                }
            });
        }

        return this.treeContextMenu;
    }, // eo function getTreeContextMenu

    /**
    * Invokes the creation of a new folder
    *
    * @private
    * @param	{Ext.tree.TreeNode}	node The node under which the new folder needs to be created
    * @returns	{Void}
    */
    invokeCreateFolder: function (node) {
        // expand the selected node, append a new childnode to it and immediately start editing it
        node.expand(false, true, function (node) {
            var newNode = node.appendChild(new Ext.tree.TreeNode({
                text: this.il8n.newFolderText,
                cls: 'x-tree-node-collapsed',
                isNew: true		// flag indicating to create a new folder rather than renaming an existing one
            }));
            this.treeEditor.ignoreNoChange = false;
            this.treeEditor.triggerEdit(newNode);
        }, this);
    }, // eo function invokeCreateFolder

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
                case 'CreateFolder':
                    // fire CreateFolder event
                    if (true !== this.eventsSuspended) {
                        this.fireEvent('CreateFolder', this, opts, o);
                    }

                    // remove flag
                    delete opts.node.attributes.isNew;
                    break;
                case 'RenameFolder':
                    // fire RenameFolder
                    if (true !== this.eventsSuspended) {
                        this.fireEvent('RenameFolder', this, opts, o);
                    }
                    break;
                case 'DeleteFolder':
                    // fire DeleteFolder event
                    if (true !== this.eventsSuspended) {
                        this.fireEvent('DeleteFolder', this, opts, o);
                    }

                    // remove node
                    opts.node.parentNode.select();
                    opts.node.remove();
                    break;
                default:
                    break;
            }
        } else {
            switch (action) {
                case 'CreateFolder':
                    // fire CreateFolderFailed event
                    if (true !== this.eventsSuspended) {
                        this.fireEvent('CreateFolderFailed', this, opts, o);
                    }

                    // remove the node
                    opts.node.remove();
                    break;
                case 'RenameFolder':
                    // fire RenameFolderFailed event
                    if (true !== this.eventsSuspended) {
                        this.fireEvent('RenameFolderFailed', this, opts, o);
                    }

                    // reset name
                    opts.node.setText(opts.oldName);
                    break;
                case 'DeleteFolder':
                    // fire DeleteFolderFailed event
                    if (true !== this.eventsSuspended) {
                        this.fireEvent('DeleteFolderFailed', this, opts, o);
                    }
                    break;
                default:
                    break;
            }
        }
    }, // eo function actionCallback

    /**
    * Create a new folder on the server
    *
    * @private
    * @param	{Ext.tree.TreeNode}	node The node representing the folder to create
    * @returns	{Void}
    */
    createFolder: function (node) {
        // fire BeforeCreateFolder event
        if (true !== this.eventsSuspended &&
		   false === this.fireEvent('BeforeCreateFolder', this, node)) {
            return;
        }

        var jsonData = {
            Action: 'CreateFolder',
            Folder: { Id: 345, Name: '', Path: '' },
            Path: node.getPath('text')
        };

        // send request to server
        Ext.Ajax.request({
            url: this.restfulUrl,
            method: 'POST',
            jsonData: jsonData,
            restful: true,
            node: node,
            callback: this.actionCallback,
            scope: this
        });
    }, // eo function createFolder

    /**
    * Renames a given folder on the server
    *
    * @private
    * @param	{Ext.tree.TreeNode}	node	The treenode that needs to be renamed
    * @param	{String}			newName	The old foldername
    * @param	{String}			oldName	The new foldername
    * @returns	{Void}
    */
    renameFolder: function (node, newName, oldName) {
        // fire BeforeRenameFolder event
        if (true !== this.eventsSuspended &&
		   false === this.fireEvent('BeforeRenameFolder', this, node, newName, oldName)) {
            return;
        }

        var jsonData = {
            Action: 'RenameFolder',
            Folder: { Id: node.id || node.data.Id, Name: oldName, Path: node.getPath('text') },
            Name: newName
        };

        // send request to server
        Ext.Ajax.request({
            url: this.restfulUrl,
            method: 'PUT',
            jsonData: jsonData,
            restful: true,
            node: node,
            newName: newName,
            oldName: oldName,
            callback: this.actionCallback,
            scope: this,
            params: {
                Action: 'RenameFolder',
                path: node.parentNode.getPath('text'),
                newName: newName,
                oldName: oldName
            }
        });

    }, // eo function renameFolder

    /**
    * Deletes folder from the server
    *
    * @private
    * @param	{Ext.tree.TreeNode}	node The treenode that needs to be deleted
    * @returns	{Void}
    */
    deleteFolder: function (node) {
        // fire BeforeDeleteFolder event
        if (true !== this.eventsSuspended &&
		   false === this.fireEvent('BeforeDeleteFolder', this, node)) {
            return;
        }

        var jsonData = {
            Action: 'DeleteFolder',
            Folder: { Id: 23423, Name: '', Path: node.getPath('text') }
        };

        // confirm removal
        Ext.Msg.show({
            title: this.il8n.confirmDeleteFolderTitleText,
            msg: String.format(this.il8n.confirmDeleteFolderMsgText, node.text),
            buttons: Ext.Msg.YESNO,
            icon: Ext.Msg.QUESTION,
            closable: false,
            scope: this,
            fn: function (buttonId) {
                if (buttonId === 'yes') {
                    // send request to server
                    Ext.Ajax.request({
                        url: this.restfulUrl,
                        method: 'DELETE',
                        jsonData: jsonData,
                        restful: true,
                        node: node,
                        callback: this.actionCallback,
                        scope: this
                    });
                }
            }
        });
    } // eo function deleteFolder
});                   // eo extend

// register xtype
Ext.reg('ux-foldertree', Ext.ux.FolderTree);

/**
* Strings for internationalization
*/
Ext.ux.FolderTree.prototype.il8n = {
    displayDateFormat: 'd/m/Y H:i',
    newText: 'New',
    deleteText: 'Delete',
    downloadText: 'Download',
    newFolderText: 'New-Folder',
    treePanelHeaderText: 'Folders',
    confirmDeleteFolderTitleText: 'Confirm folder delete',
    confirmDeleteFolderMsgText: "Are you sure you want to remove the folder '{0}' and all of it's contents?",
    actionRequestFailureTitleText: 'Oh dear..',
    actionRequestFailureMsgText: "It seems like your colleague spilled coffee on your keyboard. We can't send your request until you hang it out to dry",
    actionResponseFailureTitleText: 'PANIC!!',
    actionResponseFailureMsgText: 'Pink elephants are stampeding through the server! Run for the hills!'
};

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
