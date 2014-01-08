
/**
* @class		Ext.ux.DetailsPanel
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
Ext.define('Ext.ux.DetailsPanel', {
    extend: 'Ext.grid.Panel',
    cls: 'ux-file-details',
    layout: 'border',
    border: false,
    /**
    * @cfg {String} dataUrl The URL that is used to process data (required)
    */
    store: null,

    /**
    * Called by Ext when instantiating
    *
    * @private
    * @param {Object} config Configuration object
    */
    initComponent: function () {
        var config;
    }
});