// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import React, { Component } from 'react';
import { Image, View, Text, Switch, SectionList, TouchableOpacity, StyleSheet, Platform} from 'react-native';
//import Toast from 'react-native-simple-toast';

import Analytics from 'appcenter-analytics';

//import SharedStyles from './SharedStyles';
//import AnalyticsTabBarIcon from '../assets/analytics.png';

class App extends Component {
  
  /*
  static navigationOptions = {
    tabBarIcon: () => <Image style={{ width: 24, height: 24 }} source={AnalyticsTabBarIcon} />,
    tabBarOnPress: ({ defaultHandler, navigation }) => {
      const refreshAnalytics = navigation.getParam('refreshAnalytics');

      // Initial press: the function is not defined yet so nothing to refresh.
      if (refreshAnalytics) {
        refreshAnalytics();
      }
      defaultHandler();
    }
  }
  */

  state = {
    analyticsEnabled: false
  }

  async componentDidMount() {
    await this.refreshToggle();

    this.props.navigation.setParams({
      refreshAnalytics: this.refreshToggle.bind(this)
    });
  }

  async refreshToggle() {
    //const analyticsEnabled = true;
    this.setState({ analyticsEnabled });
  }

  render() {
    const switchRenderItem = ({ item: { title, value, toggle } }) => (
      <View style={SharedStyles.item}>
        <Text style={SharedStyles.itemTitle}>{title}</Text>
        <Switch value={this.state[value]} onValueChange={toggle} />
      </View>
    );

    const actionRenderItem = ({ item: { title, action } }) => (
      <TouchableOpacity style={SharedStyles.item} onPress={action}>
        <Text style={SharedStyles.itemButton}>{title}</Text>
      </TouchableOpacity>
    );

    //const showEventToast = eventName => Toast.show(`Scheduled event '${eventName}'.`);

    return (
      <View style={SharedStyles.container}>
        <SectionList
          renderItem={({ item }) => <Text style={[SharedStyles.item, SharedStyles.itemTitle]}>{item}</Text>}
          renderSectionHeader={({ section: { title } }) => <Text style={SharedStyles.header}>{title}</Text>}
          keyExtractor={(item, index) => item + index}
          sections={[
            {
              title: 'Settings',
              data: [
                {
                  title: 'Analytics Enabled',
                  value: 'analyticsEnabled',
                  toggle: async () => {
                    await Analytics.setEnabled(!this.state.analyticsEnabled);
                    const analyticsEnabled = await Analytics.isEnabled();
                    this.setState({ analyticsEnabled });
                  }
                },
              ],
              renderItem: switchRenderItem
            },
            {
              title: 'Actions',
              data: [
                {
                  title: 'Track event without properties',
                  action: () => {
                    const eventName = 'EventWithoutProperties';
                    Analytics.trackEvent(eventName);
                    //const analyticsEnabled = await Analytics.isEnabled();
                    //showEventToast(eventName);
                  }
                },
                {
                  title: 'Track event with properties',
                  action: () => {
                    const eventName = 'EventWithProperties';
                    Analytics.trackEvent(eventName, { property1: '100', property2: '200' });
                    //showEventToast(eventName);
                  }
                },
                {
                  title: 'Track event with long property value',
                  action: () => {
                    const eventName = 'EventWithLongProperties';
                    Analytics.trackEvent(eventName, { propertyValueTooLong: '12345678901234567890123456789012345678901234567890123456789012345' });
                    //showEventToast(eventName);
                  }
                },
              ],
              renderItem: actionRenderItem
            },
          ]}
        />
      </View>
    );
  }
}

const SharedStyles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: 'black',
    ...Platform.select({
      ios: {
        paddingTop: 25
      }
    })
  },
  header: {
    marginLeft: 10,
    marginTop: 10,
    fontSize: 12
  },
  item: {
    flex: 1,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    padding: 10,
    height: 44,
    borderWidth: StyleSheet.hairlineWidth,
    borderColor: 'black',
    backgroundColor: '#EEEEEE'
  },
  itemStretchable: {
    flex: 1,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    padding: 10,
    borderWidth: StyleSheet.hairlineWidth,
    borderColor: 'gray',
    backgroundColor: 'white'
  },
  itemTitle: {
    color: 'black',
    marginRight: 5
  },
  itemButton: {
    color: 'cornflowerblue',
    textAlign: 'center',
    width: '100%'
  },
  itemInput: {
    flex: 1,
    padding: 5,
    marginTop: -5,
    marginBottom: -5,
    textAlign: 'right'
  },
  underlinedItemInput: {
    flex: 1,
    padding: 5,
    marginTop: -5,
    marginBottom: -5,
    textAlign: 'right',
    borderBottomWidth: 1
  },
  modalSelector: {
    borderColor: 'gray',
    backgroundColor: 'white'
  },
  modalTextInput: {
    borderBottomWidth: 1,
    borderBottomColor: 'gray',
  },
  modalItem: {
    flexDirection: 'row',
    alignItems: 'center',
    marginHorizontal: 8,
    marginVertical: 8
  },
  modalButton: {
    flex: 0.5,
    padding: 16,
    borderColor: 'grey',
    borderWidth: 1,
  },
  dialogInput: {
    ...Platform.select({
      ios: {
        backgroundColor: 'lightgrey'
      }
    })
  }
});

export default App;
