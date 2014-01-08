
/**
* @class		Ext.ux.BrowserPanel
* @extends		Ext.Panel
* @namespace	Ext.ux
*
* BrowserPanel
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
Ext.define('Ext.ux.BrowserPanel', {
    extend: 'Ext.Panel',
    
    /**
    * Called by Ext when instantiating
    *
    * @private
    * @param {Object} config Configuration object
    */
    initComponent: function () {

        // Panel that displays files
        this.files = Ext.create('Ext.ux.FilePanel', {
            region: 'center',
            layout: 'card',
            activeItem: 0
        });

        // create the panel that displays folders
        this.folders = Ext.create('Ext.ux.FolderTree', {
            region: 'west',
                    width: 200,
                    autoScroll: true,
                    split: true,
                    collapseMode: 'mini',
                    selectionChange: Ext.Function.bind(this.files.onSetPath, this.files)
            });
        
        // config
        var config = Ext.apply(this.initialConfig, {
            layout: 'border',
            border: false,
            items: [this.folders, this.files]
        });

        // appy the config
        Ext.apply(this, config);

        // Call parent (required)
        Ext.ux.BrowserPanel.superclass.initComponent.apply(this, arguments);
        
    }, // eo function initComponent

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
    * Gets the full path of the current selected file
    *
    * @returns {String} The full path of the current selected file
    */
    getSelectedFilePath: function () {
        return this.files.getSelectedFilePath();
    } // eo function getSelectedFilePath
});                  // eo extend

// register xtype
//Ext.reg('ux-browserpanel', Ext.ux.BrowserPanel);

/**
* Strings for internationalization
*/
Ext.ux.BrowserPanel.prototype.il8n = {
    displayDateFormat: 'd/m/Y H:i',
    newText: 'New',
    renameText: 'Rename',
    deleteText: 'Delete',
    uploadText: 'Upload',
    downloadText: 'Download',
    viewsText: 'Views',
    detailsText: 'Details',
    thumbnailsText: 'Thumbnails',
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
